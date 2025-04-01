namespace SparkRoseDigital_Template.Common.Base;

public interface IVersionedEntity
{
    byte[] RowVersion { get; }

    void SetRowVersion(byte[] rowVersion);
}
