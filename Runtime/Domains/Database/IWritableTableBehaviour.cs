namespace StoryTime.Domains.Database
{
	internal interface IWritableTableBehaviour
    {
        string ID { set; }

        /// <summary>
        /// Name of the table that we are using.
        /// </summary>
        string TableName { set; }
        string DropdownColumn { set; }
        string LinkedID { set; }
        string LinkedColumn { set; }
        string LinkedTable { set; }
    }
}
