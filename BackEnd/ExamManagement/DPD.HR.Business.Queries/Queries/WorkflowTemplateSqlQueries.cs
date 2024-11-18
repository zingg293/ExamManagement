namespace DPD.HR.Application.Queries.Queries;

public static class WorkflowTemplateSqlQueries
{
    public const string QueryInsertWorkflowTemplate = @"INSERT INTO [dbo].[WorkflowTemplate]
           ([Id]
           ,[WorkflowName]
           ,[WorkflowCode]
           ,[Order]
           ,[StartWorkflowButton]
           ,[DefaultCompletedStatus]
           ,[TableName]
           ,[CreatedDate]
           ,[Status])
    VALUES (@Id, @WorkflowName, @WorkflowCode, @Order, @StartWorkflowButton, @DefaultCompletedStatus,
    @TableName, @CreatedDate, @Status)";

    public const string QueryUpdateWorkflowTemplate = @"UPDATE [dbo].[WorkflowTemplate] SET 
                                        WorkflowName = @WorkflowName,
                                        StartWorkflowButton = @StartWorkflowButton,
                                        DefaultCompletedStatus = @DefaultCompletedStatus
                                        WHERE Id = @Id";

    public const string QueryDeleteWorkflowTemplate = "DELETE FROM [dbo].[WorkflowTemplate] WHERE Id IN @Ids";
    public const string QueryGetByIdWorkflowTemplate = "select * from [dbo].[WorkflowTemplate] where Id = @Id";
    public const string QueryWorkflowTemplateByCode = "select * from [dbo].[WorkflowTemplate] where WorkflowCode = @WorkflowCode";
    public const string QueryGetWorkflowTemplateByIds = "select * from [dbo].[WorkflowTemplate] where Id IN @Ids";
    public const string QueryGetAllWorkflowTemplate = "select *from [dbo].[WorkflowTemplate] order by CreatedDate desc";

    public const string QueryInsertWorkflowTemplateDeleted = @"INSERT INTO [dbo].[Deleted_WorkflowTemplate]
           ([Id]
           ,[WorkflowName]
           ,[WorkflowCode]
           ,[Order]
           ,[StartWorkflowButton]
           ,[DefaultCompletedStatus]
           ,[TableName]
           ,[CreatedDate]
           ,[Status])
    VALUES (@Id, @WorkflowName, @WorkflowCode, @Order, @StartWorkflowButton, @DefaultCompletedStatus,
    @TableName, @CreatedDate, @Status)";
}