namespace DPD.HR.Application.Queries.Queries;

public static class WorkflowInstancesSqlQueries
{
    public const string QueryInsertWorkflowInstances = @"INSERT INTO [dbo].[WorkflowInstances]
                           ([Id]
                           ,[CreatedDate]
                           ,[Status]
                           ,[WorkflowCode]
                           ,[WorkflowName]
                           ,[IsCompleted]
                           ,[IsTerminated]
                           ,[IsDraft]
                           ,[NameStatus]
                           ,[TemplateId]
                           ,[CurrentStep]
                           ,[UnitId]
                           ,[ItemId]
                           ,[CreatedBy]
                           ,[RequestToChange]
                           ,[Message]
                           ,[NumberForCode]
                           ,[IsApproved])
                         VALUES (@Id, @CreatedDate, @Status, @WorkflowCode, @WorkflowName, @IsCompleted, @IsTerminated, @IsDraft,
                               @NameStatus, @TemplateId, @CurrentStep, @UnitId, @ItemId, @CreatedBy, @RequestToChange, @Message,
                               @NumberForCode, @IsApproved)";

    public const string QueryUpdateWorkflowInstances = @"UPDATE [dbo].[WorkflowInstances] SET 
                                        CurrentStep = @CurrentStep,
                                        NameStatus = @NameStatus,
                                        Message = @Message,
                                        IsDraft = @IsDraft,
                                        UnitId = @UnitId
                                        WHERE Id = @Id";
    
    public const string QueryUpdateMessageWorkflowInstances = @"UPDATE [dbo].[WorkflowInstances] SET 
                                        Message = @Message
                                        WHERE Id = @Id";
    
    public const string QueryUpdateTemplateIdWorkflowInstances = @"UPDATE [dbo].[WorkflowInstances] SET 
                                        TemplateId = @TemplateId
                                        WHERE Id = @Id";

    public const string QueryUpdateCommonInformationWorkflowInstances = @"UPDATE [dbo].[WorkflowInstances] SET 
                                        IsCompleted = @IsCompleted,
                                        IsDraft = @IsDraft,
                                        NameStatus = @NameStatus,
                                        IsTerminated = @IsTerminated,
                                        Message = @Message,
                                        UnitId = @UnitId,
                                        IsApproved = @IsApproved
                                        WHERE Id = @Id";
    
    public const string QueryUpdateWorkflowInstancesToComplete = @"UPDATE [dbo].[WorkflowInstances] SET 
                                        IsCompleted = @IsCompleted,
                                        IsDraft = @IsDraft,
                                        NameStatus = @NameStatus,
                                        Message = @Message,
                                        IsTerminated = @IsTerminated
                                        WHERE Id = @Id";

    public const string QueryDeleteWorkflowInstances = "DELETE FROM [dbo].[WorkflowInstances] WHERE Id IN @Ids";
    public const string QueryGetByIdWorkflowInstances = "select * from [dbo].[WorkflowInstances] where Id = @Id";
    public const string QueryGetWorkflowInstancesByTemplateId = "select * from [dbo].[WorkflowInstances] where TemplateId IN @TemplateId";
    public const string QueryGetWorkflowInstancesByItemId = "select * from [dbo].[WorkflowInstances] where ItemId IN @ItemId";
    public const string QueryGetAllWorkflowInstances = "select *from [dbo].[WorkflowInstances] order by CreatedDate desc";
    public const string QueryMaxNumberForCodeWorkflowInstances = "SELECT COALESCE(MAX(NumberForCode), 0) AS MaxNumber FROM WorkflowInstances WHERE TemplateId = @TemplateId";

    public const string QueryInsertWorkflowInstancesDeleted = @"INSERT INTO [dbo].[Deleted_WorkflowInstances]
                           ([Id]
                           ,[CreatedDate]
                           ,[Status]
                           ,[WorkflowCode]
                           ,[WorkflowName]
                           ,[IsCompleted]
                           ,[IsTerminated]
                           ,[IsDraft]
                           ,[NameStatus]
                           ,[TemplateId]
                           ,[CurrentStep]
                           ,[UnitId]
                           ,[ItemId]
                           ,[CreatedBy]
                           ,[RequestToChange]
                           ,[Message]
                           ,[NumberForCode]
                           ,[IsApproved])
                         VALUES (@Id, @CreatedDate, @Status, @WorkflowCode, @WorkflowName, @IsCompleted, @IsTerminated, @IsDraft,
                               @NameStatus, @TemplateId, @CurrentStep, @UnitId, @ItemId, @CreatedBy, @RequestToChange, @Message,
                               @NumberForCode, @IsApproved)";
}