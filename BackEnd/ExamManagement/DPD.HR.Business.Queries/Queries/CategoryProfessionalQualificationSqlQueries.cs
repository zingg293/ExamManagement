namespace DPD.HR.Application.Queries.Queries
{
    public static class CategoryProfessionalQualificationSqlQueries
    {
        public const string QueryInsertCategoryProfessionalQualification = @"
        INSERT INTO [dbo].[CategoryProfessionalQualification]
                   ([Id]
                   ,[CreatedDate]
                   ,[Status]
                   ,[NameProfessionalQualification])
             VALUES
                   (@Id
                   ,@CreatedDate
                   ,@Status
                   ,@NameProfessionalQualification)";
        public const string QueryUpdateCategoryProfessionalQualification = @"
            UPDATE [dbo].[CategoryProfessionalQualification]
            SET 
                [NameProfessionalQualification] = @NameProfessionalQualification
            WHERE [Id] = @Id";
        public const string QueryGetByIdCategoryProfessionalQualification = @"select * from [dbo].[CategoryProfessionalQualification] where Id = @Id";
        public const string QueryGetCategoryProfessionalQualificationByIds = @"select * from [dbo].[CategoryProfessionalQualification] where Id IN @Ids";
        public const string QueryDeleteCategoryProfessionalQualification = @"Delete from [dbo].[CategoryProfessionalQualification] where Id IN @Ids";
        public const string QueryGetAllCategoryProfessionalQualificationAvailable = @"";
        public const string QueryGetAllCategoryProfessionalQualification = @"SELECT * FROM [dbo].[CategoryProfessionalQualification] order by [NameProfessionalQualification]";
        public const string QueryHideCategoryProfessionalQualification = @"";
        public const string QueryInsertCategoryProfessionalQualificationDeleted = @"
        INSERT INTO [dbo].[Deleted_CategoryProfessionalQualification]
                   ([Id]
                   ,[CreatedDate]
                   ,[Status]
                   ,[NameProfessionalQualification])
             VALUES
                   (@Id
                   ,@CreatedDate
                   ,@Status
                   ,@NameProfessionalQualification)";
    }
}
