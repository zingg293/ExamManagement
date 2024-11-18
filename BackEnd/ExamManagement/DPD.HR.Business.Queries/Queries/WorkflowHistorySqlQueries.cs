namespace DPD.HR.Application.Queries.Queries;

public static class WorkflowHistorySqlQueries
{
    public const string QueryInsertWorkflowHistory = @"INSERT INTO [dbo].[WorkflowHistory]
           ([Id]
           ,[IdWorkFlowInstance]
           ,[IdUser]
           ,[Action]
           ,[IdUnit]
           ,[CreatedDate]
           ,[Status]
           ,[IsStepCompleted]
           ,[Comment]
           ,[Message]
           ,[IsCancelled]
           ,[IsRequestToChanged])
                         VALUES (@Id, @IdWorkFlowInstance, @IdUser, @Action, @IdUnit, @CreatedDate, @Status, @IsStepCompleted,
                                    @Comment, @Message, @IsCancelled, @IsRequestToChanged)";

    public const string QueryDeleteWorkflowHistory = "DELETE FROM [dbo].[WorkflowHistory] WHERE Id IN @Ids";

    public const string QueryGetWorkflowHistoriesByIdWorkFlowInstance =
        "select * from [dbo].[WorkflowHistory] where IdWorkFlowInstance IN @IdWorkFlowInstance order by CreatedDate desc";
    public const string QueryGetAllWorkflowHistories =
        "select * from [dbo].[WorkflowHistory]";
    public const string QueryGetAllWorkflowHistoriesByIdWorkFlowInstance =
        "select * from [dbo].[WorkflowHistory] where IdWorkFlowInstance IN @IdWorkFlowInstance";

    public const string QueryGetAllWorkflowHistoryByIdUserAndIdWorkFlowInstance =
        "select * from WorkflowHistory where IdUser = @IdUser and IdWorkFlowInstance = @IdWorkFlowInstance";

    public const string QueryInsertWorkflowHistoryDeleted = @"INSERT INTO [dbo].[Deleted_WorkflowHistory]
           ([Id]
           ,[IdWorkFlowInstance]
           ,[IdUser]
           ,[Action]
           ,[IdUnit]
           ,[CreatedDate]
           ,[Status]
           ,[IsStepCompleted]
           ,[Comment]
           ,[Message]
           ,[IsCancelled]
           ,[IsRequestToChanged])
                         VALUES (@Id, @IdWorkFlowInstance, @IdUser, @Action, @IdUnit, @CreatedDate, @Status, @IsStepCompleted,
                                    @Comment, @Message, @IsCancelled, @IsRequestToChanged)";
}