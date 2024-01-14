namespace Data_Logger_1._3.Models
{
    public class CheckListItem
    {
        /* PROPERTIES */


        public bool IsChecked { get; set; } = false;

        public string Item { get; set; } = "";




        /* CONSTRUCTORS */



        public CheckListItem() { }

        public CheckListItem(bool isChecked, string item)
        {
            IsChecked = isChecked;
            Item = item;
        }



        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is CheckListItem item &&
                   IsChecked == item.IsChecked &&
                   Item == item.Item;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IsChecked, Item);
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
