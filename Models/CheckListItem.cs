namespace Data_Logger_1._3.Models
{

    /// <summary>
    /// The class for creating checklists. 
    /// These items are stored within a NoteItem log.
    /// </summary>
    public class CheckListItem
    {
        /* PROPERTIES */

        public int ID { get; set; } = -1;

        public bool IsChecked { get; set; } = false;

        public string Item { get; set; } = "";




        /* CONSTRUCTORS */



        public CheckListItem() { }

        public CheckListItem(bool isChecked, string item)
        {
            IsChecked = isChecked;
            Item = item;
        }

        public CheckListItem(int id, bool isChecked, string item)
        {
            ID = id;
            IsChecked = isChecked;
            Item = item;
        }



        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is CheckListItem item &&
                   ID == item.ID &&
                   IsChecked == item.IsChecked &&
                   Item == item.Item;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, IsChecked, Item);
        }

        public static bool operator ==(CheckListItem left, CheckListItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CheckListItem left, CheckListItem right)
        {
            return !left.Equals(right);
        }
    }
}
