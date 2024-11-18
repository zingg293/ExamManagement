namespace DPD.HR.Application.Queries.Queries;

public static class CandidateSqlQueries
{
    public const string QueryInsertCandidate = @"INSERT INTO [dbo].[Candidate]
           ([Id]
           ,[Name]
           ,[Sex]
           ,[Birthday]
           ,[Phone]
           ,[Email]
           ,[Address]
           ,[IdCity]
           ,[IdDistrict]
           ,[IdWard]
           ,[Note]
           ,[Avatar]
           ,[Status]
           ,[CreatedDate]
           ,[File])
            VALUES (@Id, @Name, @Sex, @Birthday, @Phone, @Email, @Address, @IdCity, @IdDistrict, @IdWard, @Note, @Avatar, @Status,
                       @CreatedDate, @File)";

    public const string QueryUpdateCandidate = @"UPDATE [dbo].[Candidate] SET 
                            Name = @Name,
                            Sex = @Sex,
                            Birthday = @Birthday,
                            Phone = @Phone,
                            Email = @Email,
                            [Address] = @Address,
                            IdCity = @IdCity,
                            IdDistrict = @IdDistrict,
                            IdWard = @IdWard,
                            Note = @Note,
                            Avatar = @Avatar
                            WHERE Id = @Id";

    public const string QueryDeleteCandidate = "DELETE FROM [dbo].[Candidate] WHERE Id IN @Ids";
    public const string QueryGetByIdCandidate = "select * from [dbo].[Candidate] where Id = @Id";
    public const string QueryGetCandidateByIds = "select * from [dbo].[Candidate] where Id IN @Ids";
    public const string QueryGetAllCandidate = "select *from [dbo].[Candidate] order by CreatedDate desc";

    public const string QueryInsertCandidateDeleted = @"INSERT INTO [dbo].[Deleted_Candidate]
           ([Id]
           ,[Name]
           ,[Sex]
           ,[Birthday]
           ,[Phone]
           ,[Email]
           ,[Address]
           ,[IdCity]
           ,[IdDistrict]
           ,[IdWard]
           ,[Note]
           ,[Avatar]
           ,[Status]
           ,[CreatedDate]
           ,[File])
            VALUES (@Id, @Name, @Sex, @Birthday, @Phone, @Email, @Address, @IdCity, @IdDistrict, @IdWard, @Note, @Avatar, @Status,
                       @CreatedDate, @File)";
}