using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls
{
    public class Item
    {
        protected string name; // The name of the item.
        protected double price; // The price of the item;
        protected string imagepath; // The path to the image within the project directory. e.g. "Images/mochi.png"
        protected string altimagepath; // The path to the alternate image (primarily for hover/selection images).

        // Null Constructor
        public Item() { }
        // <Important>
        // Main Constructor: Takes a name and image path as arguments.
        // An alternate image path can be provided, otherwise it is set to the main image.
        public Item(string n, string i, double p = 0, string a = "")
        {
            this.name = n;
            this.price = p;
            this.imagepath = i;
            if (a == "") // if no alternate image is provided
                this.altimagepath = i;
            else
                this.altimagepath = a;
        }

        // <Important>
        // Sets the Source of the given Image object to this item's image path.
        // By default, displays non-alternate image. Display alternate image by calling SetImage(Image i, true).
        public void SetImage(Image i, Boolean alt = false)
        {
            if (alt)
                i.Source = new BitmapImage(new Uri(this.AltImage(), UriKind.Relative));
            else
                i.Source = new BitmapImage(new Uri(this.Image(), UriKind.Relative));
        }
        // Sets the content of the given label to this item's name.
        public void SetLabel(Label l)
        {
            l.Content = this.name;
        }
        //// ACCESSOR FUNCTIONS
        // <Important>
        public string Name()
        {
            return name;
        }
        // <Important>
        public double Price()
        {
            return price;
        }
        public string Image()
        {
            return imagepath;
        }
        public string AltImage()
        {
            return altimagepath;
        }
    }
    public class ItemList
    {
        protected List<Item> items; // Contains the items.
        protected int selected = 0; // The index of the currently selected item.
        public Boolean[] multiselected; // A boolean array where corresponding t/f values determine if an item is multi-selected. (all false by default)
        private Item nullitem = new Item("", ""); // Creates a null item. Set the image path to that of a blank image!

        // Null Constructor
        public ItemList()
        {
            items = new List<Item>();
            multiselected = new Boolean[0];
        }
        // <Important>
        // Main Constructor
        public ItemList(Item[] i)
        {
            items = new List<Item>(i);
            multiselected = new Boolean[i.Count()];
        }

        //// GENERAL METHODS
        // <Important>
        // Adds Item to the ItemList.
        public void Add(Item i)
        {
            items.Add(i);

            Boolean[] newms = new Boolean[multiselected.Count() + 1]; // need to reconstruct boolean array...
            for (int j = 0; j < multiselected.Count(); j++)
            {
                newms[j] = multiselected[j];
            }
            multiselected = newms;
        }

        public void Delete(Item i)
        {
            items.Remove(i);
        }
        // <Important>
        // Displays an ItemList on the gives array of Images.
        // Centers first item on the middle Image, and displays nullitem's image for anything out of bounds.
        // Default (displaytype = 0): Non-selected items will display their alternate images.
        // displaytype = 1: All items will display their main images. alt = -1: All items will display their alternate images.
        public void Display(Image[] img, int displaytype = 0)
        {
            for (int i = 0; i < img.Count(); i++)
            {
                int newindex = i + selected; // basically, center the first Item on middle Image
                if (displaytype == 0)
                    this.Item(newindex).SetImage(img[i], (newindex == selected) || MultiSelected(newindex));
                else if (displaytype == -1)
                    this.Item(newindex).SetImage(img[i]);
                else
                    this.Item(newindex).SetImage(img[i], true);
            }
        }
        // <Important>
        // Displays an ItemList using the names of the items, on the given array of labels.
        // Currently should be used for size and options (until we get images for those).
        public void DisplayAsText(Label[] labels)
        {
            for (int i = 0; i < labels.Count(); i++) 
            {
                int newindex = i + selected;
                this.Item(newindex).SetLabel(labels[i]);
            }
        }

        public int SelectedIndex()
        {
            return selected;
        }
        //// SINGLE SELECTION
        // <Important>
        // Moves selection up.
        public void Up()
        {
            if (selected > 0)
                selected--;
        }
        // <Important>
        // Moves selection down.
        public void Down()
        {
            if (selected < items.Count - 1)
                selected++;
        }
        // Changes selection to the index i.
        public void Select(int i)
        {
            if (i < items.Count && i >= 0)
            {
                selected = i;
            }
        }
        // Returns the currently selected item.
        public Item ReturnSingle()
        {
            if (selected < items.Count())
            {
                return items[selected];
            }
            else
            {
                return null;
            }
        }
        //// MULTIPLE SELECTION
        // Toggles the multi-selection status of the ith item.
        public void Toggle(int i)
        {
            if (i < items.Count && i >= 0)
            {
                this.multiselected[i] = !this.multiselected[i];
            }
        }
        // <Important>
        // Toggles the multi-selection status of the currently (single) selected item.
        public void ToggleSelected()
        {
            this.multiselected[selected] = !this.multiselected[selected];
        }
        // <Important>
        // Returns all multi-selected items in a new ItemList.
        public ItemList ReturnMultiple()
        {
            ItemList newlist = new ItemList();
            for (int i = 0; i < this.Count(); i++)
            {
                if (multiselected[i])
                {
                    newlist.Add(this.Item(i));
                }
            }
            return newlist;
        }

        //// ACCESSOR FUNCTIONS
        // Returns the List of Items.
        public List<Item> Items()
        {
            return items;
        }
        // Returns the Item at index i.
        public Item Item(int i)
        {
            if (i < Count() && i >= 0)
            {
                return items[i];
            }
            else
            {
                return nullitem;
            }
        }
        // Returns true if Item i is multi-selected.
        public Boolean MultiSelected(int i)
        {
            if (i < Count() && i >= 0)
                return multiselected[i];
            else
                return false;
        }
        // Returns the number of Items.
        public int Count()
        {
            return items.Count();
        }
        // Returns the names of all Items as a string array.
        public string[] Names()
        {
            string[] names = new string[this.Count()];
            for (int i = 0; i < this.Count(); i++)
            {
                names[i] = this.Item(i).Name();
            }
            return names;
        }
        // Returns the number of Items.
        public double TotalPrice()
        {
            double sum = 0;
            foreach (Item i in Items())
            {
                sum += i.Price();
            }
            return sum;
        }
    }


    public class Category : Item
    {
        protected ItemList foods; // Contains the possible Foods for a category.

        // Main Constructor
        public Category(string n, string i, ItemList f, string a = "")
        {
            this.name = n;
            this.imagepath = i;
            this.foods = f;
            if (a == "") // if no alternate image is provided
                this.altimagepath = i;
            else
                this.altimagepath = a;
        }

        // Navigation Functions (For sub-lists)
        public void FoodUp()
        {
            foods.Up();
        }
        public void FoodDown()
        {
            foods.Down();
        }

        // Display Function (For sub-lists)
        public void DisplayFoods(Image[] img, int displaytype = 0)
        {
            Foods().Display(img, displaytype);
        }

        // Accessor Functions
        public ItemList Foods()
        {
            return foods;
        }
        public Food Food() // Returns the selected Food.
        {
            return (Food)foods.ReturnSingle();
        }
        public string FoodName() // Returns the name of the selected Food.
        {
            return Food().Name();
        }
        public double FoodPrice() // Returns the price of the selected Food.
        {
            return Food().Price();
        }
    }
    public class Food : Item
    {
        // Note: Base price of item is stored in price!
        protected ItemList sizes; // Contains the possible sizes for a Food. Cost of each size is stored in price!
        protected ItemList amounts; // Contains the possible amounts for a Food. Amount is stored in price!
        protected ItemList options; // Contains the possible options for a Food. Cost of adding option is stored in price!

        // Main Constructor
        public Food(string n, string i, ItemList s, ItemList a, ItemList o, double p = 0, string alt = "")
        {
            this.name = n;
            this.price = p;
            this.imagepath = i;
            this.sizes = s;
            this.amounts = a;
            this.options = o;
            if (alt == "") // if no alternate image is provided
                this.altimagepath = i;
            else
                this.altimagepath = alt;
        }

        // Navigation Functions (For sub-lists)
        public void SizeUp()
        {
            sizes.Up();
        }
        public void SizeDown()
        {
            sizes.Down();
        }
        public void AmountUp()
        {
            amounts.Up();
        }
        public void AmountDown()
        {
            amounts.Down();
        }
        public void OptionUp()
        {
            options.Up();
        }
        public void OptionDown()
        {
            options.Down();
        }
        public void OptionToggle()
        {
            options.ToggleSelected();
        }

        // Display Functions (For sub-lists)
        public void DisplaySizes(Image[] img, int displaytype = 0)
        {
            Sizes().Display(img, displaytype);
        }
        public void DisplaySizes(Label[] labels)
        {
            Sizes().DisplayAsText(labels);
        }
        public void DisplayAmounts(Image[] img, int displaytype = 0)
        {
            Amounts().Display(img, displaytype);
        }
        public void DisplayAmounts(Label[] labels)
        {
            Amounts().DisplayAsText(labels);
        }
        public void DisplayOptions(Image[] img, int displaytype = 0)
        {
            Options().Display(img, displaytype);
        }
        public void DisplayOptions(Label[] labels)
        {
            Options().DisplayAsText(labels);
        }

        // Accessor Functions
        public double TotalPrice()
        {
            double amount = 1;
            double sizePrice = 0;
            double optionsPrice = 0;
            if (sizes != null)
            {
                sizePrice = sizes.ReturnSingle().Price();
            }
            if (options != null)
            {
                if (Option() != null)
                {
                    optionsPrice = Option().Price();
                }
            }
            if (Amounts() != null)
            {
                amount = Amount();
            }
            return (price + sizePrice + optionsPrice) * amount;
        }
        public ItemList Sizes()
        {
            return sizes;
        }
        public ItemList Amounts()
        {
            return amounts;
        }
        public ItemList Options(Boolean onlyselected = false)
        {
            if (onlyselected)
                return options.ReturnMultiple();
            else
                return options;
        }
        public Item Option()
        {
            return options.ReturnSingle();
        }
        public string SizeName() // Returns the name of the selected Size.
        {
            if (sizes != null)
                return sizes.ReturnSingle().Name();
            return "";
        }
        public double Amount() // Returns the price (amount) of the selected Amount.
        {
            if (amounts != null)
                return amounts.ReturnSingle().Price();
            return 0;
        }
        public string[] OptionNames() // Returns the names of the selected Options.
        {
            if (options != null)
                return Options().Names();
            return new String[0];
        }
    }
}
