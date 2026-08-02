using GomMessage.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GomMessage.Infrastructure.Data.Converters;

internal static class ValueObjectConversionExtensions
{
    public static PropertyBuilder<EmailAddress> HasEmailAddressConversion(this PropertyBuilder<EmailAddress> builder)
        => builder.HasConversion(
            email => email.Value,
            value => EmailAddress.Create(value));

    public static PropertyBuilder<EmailAddress?> HasNullableEmailAddressConversion(this PropertyBuilder<EmailAddress?> builder)
        => builder.HasConversion(
            email => email == null ? (string?)null : email.Value,
            value => string.IsNullOrWhiteSpace(value) ? (EmailAddress?)null : EmailAddress.Create(value));

    public static PropertyBuilder<TenantSlug> HasTenantSlugConversion(this PropertyBuilder<TenantSlug> builder)
        => builder.HasConversion(
            slug => slug.Value,
            value => TenantSlug.Create(value));

    public static PropertyBuilder<JsonContent?> HasNullableJsonContentConversion(this PropertyBuilder<JsonContent?> builder)
        => builder.HasConversion(
            json => json == null ? (string?)null : json.Value,
            value => string.IsNullOrWhiteSpace(value) ? (JsonContent?)null : JsonContent.Create(value));

    public static PropertyBuilder<CronExpression?> HasNullableCronExpressionConversion(this PropertyBuilder<CronExpression?> builder)
        => builder.HasConversion(
            cron => cron == null ? (string?)null : cron.Value,
            value => string.IsNullOrWhiteSpace(value) ? (CronExpression?)null : CronExpression.Create(value));

    public static PropertyBuilder<Score?> HasNullableScoreConversion(this PropertyBuilder<Score?> builder)
        => builder.HasConversion(
            score => score == null ? (double?)null : score.Value,
            value => value.HasValue ? Score.Create(value.Value) : (Score?)null);

    public static PropertyBuilder<ConfidenceScore?> HasNullableConfidenceScoreConversion(this PropertyBuilder<ConfidenceScore?> builder)
        => builder.HasConversion(
            confidence => confidence == null ? (double?)null : confidence.Value,
            value => value.HasValue ? ConfidenceScore.Create(value.Value) : (ConfidenceScore?)null);
}