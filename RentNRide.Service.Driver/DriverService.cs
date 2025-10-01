using Microsoft.EntityFrameworkCore;
using Minio;
using Minio.DataModel.Args;
using RentNRide.Common.Domain.Exceptions;
using RentNRide.Common.Domain.Models.Driver;
using RentNRide.Common.Domain.Services;
using RentNRide.Common.Id;
using RentNRide.Common.Validation;
using RentNRide.Data.Entities;
using RentNRide.Data.Entities.Driver;
using RentNRide.Service.Motorcycle.Validators;

namespace RentNRide.Service.Driver;

public class DriverService : IDriverService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMinioClient minioClient;
    private readonly string bucketName = "driver-cnh";

    public DriverService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<DriverModel>> GetAll()
    {
        return await unitOfWork
            .DriverRepository
            .AsQueryable()
            .Select(s => new DriverModel
            {
                DriverId = s.DriverId,
                Name = s.Name,
                BirthDate = s.BirthDate,
                Cnpj = s.Cnpj,
                LicenseNumber = s.LicenseNumber,
                LicenseType = s.LicenseType,
                LicenseImageUrl = s.LicenseImageUrl
            }).ToListAsync();
    }

    public async Task<DriverModel> GetById(string driverId)
    {
        return await unitOfWork
             .DriverRepository
             .Where(w => w.DriverId == driverId)
             .Select(s => new DriverModel
             {
                 DriverId = s.DriverId,
                 Name = s.Name,
                 BirthDate = s.BirthDate,
                 Cnpj = s.Cnpj,
                 LicenseNumber = s.LicenseNumber,
                 LicenseType = s.LicenseType,
                 LicenseImageUrl = s.LicenseImageUrl
             }).FirstOrDefaultAsync() ?? throw new NotFoundException("Entregador não encontrado");
    }

    public async Task<string> CreateAsync(DriverCreateModel model)
    {
        #region Validations

        model.FormatData();

        await new CreateValidator().Run(model);

        if (await unitOfWork.DriverRepository.AnyAsync(m => m.Cnpj == model.Cnpj))
            throw new ApiException("O CNPJ informado já está vinculado a outro entregador.");

        if (await unitOfWork.DriverRepository.AnyAsync(m => m.LicenseNumber == model.LicenseNumber))
            throw new ApiException("O número da CNH informado já está vinculado a outro entregador.");

        #endregion

        var newId = await IdGenerator.NewSync(6);

        var driver = new Data.Entities.Driver.Driver()
        {
            DriverId = newId,
            Name = model.Name,
            Cnpj = model.Cnpj,
            LicenseNumber = model.LicenseNumber,
            LicenseType = model.LicenseType,
            BirthDate = model.BirthDate
        };

        // Updates URL
        if (!string.IsNullOrEmpty(model.Base64Image))
        {
            var url = await UpdateFile(newId, model.Base64Image);
            driver.LicenseImageUrl = url;
        }

        unitOfWork.DriverRepository.Add(driver);

        await unitOfWork.SaveChangesAsync();

        return newId;
    }

    public async Task UpdateDriveLicenseAsync(string driverId, UploadLicenseModel model)
    {
        var driver = await unitOfWork
             .DriverRepository
             .Where(w => w.DriverId == driverId)
             .Select(s => new DriverModel
             {
                 DriverId = s.DriverId,
                 Name = s.Name,
                 BirthDate = s.BirthDate,
                 Cnpj = s.Cnpj,
                 LicenseNumber = s.LicenseNumber,
                 LicenseType = s.LicenseType,
                 LicenseImageUrl = s.LicenseImageUrl
             }).FirstOrDefaultAsync() ?? throw new NotFoundException("Entregador não encontrado");

        // Validation
        await new Validators.DriveLicenseValidator().Run(model);

        // Updates URL
        var url = await UpdateFile(driverId, model.Base64Image);
        driver.LicenseImageUrl = url;

        await unitOfWork.SaveChangesAsync();
    }

    private async Task<string> UpdateFile(string driverId, string base64Image)
    {
        string extension = string.Empty;

        if (base64Image.StartsWith("data:image/png;base64,"))
        {
            extension = ".png";
            base64Image = base64Image.Replace("data:image/png;base64,", "");
        }
        else if (base64Image.StartsWith("data:image/bmp;base64,"))
        {
            extension = ".bmp";
            base64Image = base64Image.Replace("data:image/bmp;base64,", "");
        }

        // get bytes from Base64
        byte[] imageBytes;
        try
        {
            imageBytes = Convert.FromBase64String(base64Image);
        }
        catch
        {
            throw new ApiException("Imagem em Base64 inválida.");
        }

        // create minIO Bucket
        bool hasBucket = await minioClient
            .BucketExistsAsync(new BucketExistsArgs()
            .WithBucket(bucketName));

        if (!hasBucket)
        {
            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
        }

        var objectName = $"{driverId}/cnh{extension}";

        // Upload to MinIO
        using (var stream = new MemoryStream(imageBytes))
        {
            await minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(extension == ".png" ? "image/png" : "image/bmp"));
        }

        return $"http://localhost:9000/{bucketName}/{objectName}";
    }
}
