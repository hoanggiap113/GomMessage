using GomMessage.Domain.Common;
using GomMessage.Domain.Entities.Enums;
using GomMessage.Domain.Events;
using GomMessage.Domain.ValueObjects;

namespace GomMessage.Domain.Entities;

public sealed class Job : AuditableEntity
{
    public Guid TenantId { get; private set; }
    public JobType JobType { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public JsonContent? InputChannelIds { get; private set; }
    public string? RulesContent { get; private set; }
    public JsonContent? RulesConfig { get; private set; }
    public JsonContent? SkipConditions { get; private set; }
    public string AiProvider { get; private set; } = string.Empty;
    public string AiModel { get; private set; } = string.Empty;
    public JsonContent? Outputs { get; private set; }
    public JobScheduleType ScheduleType { get; private set; }
    public CronExpression? ScheduleCron { get; private set; }
    public bool Active { get; private set; }
    public DateTimeOffset? LastRunAt { get; private set; }
    public string? LastRunStatus { get; private set; }

    private Job()
    {
    }

    private Job(
        Guid id,
        Guid tenantId,
        JobType jobType,
        string name,
        string? description,
        JsonContent? inputChannelIds,
        string? rulesContent,
        JsonContent? rulesConfig,
        JsonContent? skipConditions,
        string aiProvider,
        string aiModel,
        JsonContent? outputs,
        JobScheduleType scheduleType,
        CronExpression? scheduleCron) : base(id)
    {
        TenantId = Guard.AgainstEmptyGuid(tenantId, nameof(tenantId));
        JobType = jobType;
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Description = string.IsNullOrWhiteSpace(description) ? null : description;
        InputChannelIds = inputChannelIds;
        RulesContent = string.IsNullOrWhiteSpace(rulesContent) ? null : rulesContent;
        RulesConfig = rulesConfig;
        SkipConditions = skipConditions;
        AiProvider = Guard.AgainstNullOrWhiteSpace(aiProvider, nameof(aiProvider));
        AiModel = Guard.AgainstNullOrWhiteSpace(aiModel, nameof(aiModel));
        Outputs = outputs;
        ScheduleType = scheduleType;
        ScheduleCron = scheduleCron;
        Active = true;
        MarkCreated();
    }

    public static Job Create(
        Guid tenantId,
        JobType jobType,
        string name,
        string aiProvider,
        string aiModel,
        JobScheduleType scheduleType,
        string? scheduleCron = null,
        string? description = null,
        string? inputChannelIdsJson = null,
        string? rulesContent = null,
        string? rulesConfigJson = null,
        string? skipConditionsJson = null,
        string? outputsJson = null)
    {
        var job = new Job(
            Guid.NewGuid(),
            tenantId,
            jobType,
            name,
            description,
            JsonContent.CreateOrNull(inputChannelIdsJson),
            rulesContent,
            JsonContent.CreateOrNull(rulesConfigJson),
            JsonContent.CreateOrNull(skipConditionsJson),
            aiProvider,
            aiModel,
            JsonContent.CreateOrNull(outputsJson),
            scheduleType,
            string.IsNullOrWhiteSpace(scheduleCron) ? null : CronExpression.Create(scheduleCron));

        job.AddDomainEvent(new JobCreatedDomainEvent(job.Id, job.TenantId, job.JobType.ToString(), job.Name));
        return job;
    }

    public void UpdateDefinition(string name, string? description, string? rulesContent, string? rulesConfigJson, string? skipConditionsJson, string? outputsJson)
    {
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Description = string.IsNullOrWhiteSpace(description) ? null : description;
        RulesContent = string.IsNullOrWhiteSpace(rulesContent) ? null : rulesContent;
        RulesConfig = JsonContent.CreateOrNull(rulesConfigJson);
        SkipConditions = JsonContent.CreateOrNull(skipConditionsJson);
        Outputs = JsonContent.CreateOrNull(outputsJson);
        Touch();
    }

    public void UpdateAiModel(string aiProvider, string aiModel)
    {
        AiProvider = Guard.AgainstNullOrWhiteSpace(aiProvider, nameof(aiProvider));
        AiModel = Guard.AgainstNullOrWhiteSpace(aiModel, nameof(aiModel));
        Touch();
    }

    public void UpdateSchedule(JobScheduleType scheduleType, string? scheduleCron)
    {
        ScheduleType = scheduleType;
        ScheduleCron = string.IsNullOrWhiteSpace(scheduleCron) ? null : CronExpression.Create(scheduleCron);
        Touch();
    }

    public void Activate()
    {
        Active = true;
        Touch();
    }

    public void Deactivate()
    {
        Active = false;
        Touch();
    }

    public void MarkLastRun(JobRunStatus status, DateTimeOffset? runAt = null)
    {
        LastRunStatus = status.ToString();
        LastRunAt = runAt ?? DateTimeOffset.UtcNow;
        Touch();
    }
}
