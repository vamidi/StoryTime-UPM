namespace StoryTime.Domains.Database
{
    public interface IReadableTableBehaviour
    {
        string ID { get; }

        /// <summary>
        /// Name of the table that we are using.
        /// </summary>
        string TableName { get; }
        string DropdownColumn { get; }
        string LinkedID { get; }
        string LinkedColumn { get; }
        string LinkedTable { get; }
    }
}
