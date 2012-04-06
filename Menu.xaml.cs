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
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public const int CATEGORY = 0;
        public const int ITEM = 1;
        public const int SIZE = 2;
        public const int AMOUNT = 3;
        public const int OPTIONS = 4;

        public Menu()
        {
            InitializeComponent();
        }

        /* Menu layout: 
         * MEALS
         * Hamburger: regular = $4.00, jumbo = $5.50
         * options: cheese = $1.00, pickles = $0.00, onions = $0.00
         * Chicken sandwich: regular = $4.50, jumbo = $6.00
         * options: same as hamburger
         * Big salad: one size = $5.00
         * options: cheese = $1.00, ranch dressing = $0.00, italian dressing = $0.00
         * SIDES
         * Fries: small = $1.50, medium = $2.50, large = $3.00
         * options: chili-cheese: $1.50
         * Salad: one size = $2.50
         * options: ranch dressing = $0.00, italian dressing = $0.00
         * Fruit: one size = $3.00
         * DRINKS
         * Soda: small = $1.50, medium = $2.00, large = $2.50
         * options: coke = $0.00, pepsi = $0.00, sprite = $0.00
         * Boba: small = $2.00, medium = $2.50, large = $3.00
         * options: boba = $0.00
         * Water: one size = $0.00
         */

        Food burger, chicksandwich, bigsalad, fries, sidesalad, fruit, soda, boba, water;
        ItemList menu;
        Category meals, sides, drinks;
        Image[] catCol, itemCol, amountCol;
        Label[] sizeCol, optionsCol;
        int currentCol;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Amounts
            Item one = new Item("One", "Images/number1.jpg");
            Item two = new Item("Two", "Images/number2.jpg");
            Item three = new Item("Three", "Images/number3.jpg");
            ItemList allAmounts = new ItemList(new Item[] { one, two, three });
            // Food Sizes
            Item burgerRegSize = new Item("Regular", null, 4.0);
            Item burgerJumboSize = new Item("Jumbo", null, 5.5);
            Item sandwichRegSize = new Item("Regular", null, 4.5);
            Item sandwichJumboSize = new Item("Jumbo", null, 6.0);
            Item friesSmallSize = new Item("Small", null, 1.5);
            Item friesMedSize = new Item("Medium", null, 2.5);
            Item friesLargeSize = new Item("Large", null, 3.0);
            Item sodaSmallSize = new Item("Small", null, 1.5);
            Item sodaMedSize = new Item("Medium", null, 2.0);
            Item sodaLargeSize = new Item("Large", null, 2.5);
            Item bobaSmallSize = new Item("Small", null, 2.0);
            Item bobaMedSize = new Item("Medium", null, 2.5);
            Item bobaLargeSize = new Item("Large", null, 3.0);
            // Food Options
            Item cheeseOption = new Item("Cheese", null, 1.0);
            Item picklesOption = new Item("Pickles", null, 0.0);
            Item onionsOption = new Item("Onions", null, 0.0);
            Item ranchOption = new Item("Ranch Dressing", null, 0.0);
            Item italianOption = new Item("Italian Dressing", null, 0.0);
            Item chiliCheeseOption = new Item("Chili Cheese", null, 1.5);
            Item cokeOption = new Item("Coke", null, 0.0);
            Item pepsiOption = new Item("Pepsi", null, 0.0);
            Item spriteOption = new Item("Sprite", null, 0.0);
            Item bobaOption = new Item("With Pearls", null, 0.0);
            // Foods
            // Meals
            ItemList burgerSizes = new ItemList(new Item[] { burgerRegSize, burgerJumboSize });
            ItemList burgerOptions = new ItemList(new Item[] { cheeseOption, picklesOption, onionsOption });
            burger = new Food("Hamburger", "Images/hamburger.jpg", burgerSizes, allAmounts, burgerOptions); //base price needed?
            ItemList sandwichSizes = new ItemList(new Item[] { sandwichRegSize, sandwichJumboSize });
            chicksandwich = new Food("Chicken Sandwich", "Images/chicken_sandwich.jpg", sandwichSizes, allAmounts, burgerOptions);
            ItemList saladOptions = new ItemList(new Item[] { cheeseOption, italianOption, ranchOption });
            bigsalad = new Food("Big Salad", "Images/big_salad.jpg", null, allAmounts, saladOptions, 5.0);
            // Sides
            ItemList friesSizes = new ItemList(new Item[] { friesSmallSize, friesMedSize, friesLargeSize });
            ItemList friesOptions = new ItemList(new Item[] { chiliCheeseOption });
            fries = new Food("French Fries", "Images/french_fries.png", friesSizes, allAmounts, friesOptions);
            sidesalad = new Food("Side Salad", "Images/side_salad.png", null, allAmounts, saladOptions, 2.5);
            fruit = new Food("Fruit Salad", "Images/fruit_cup.png", null, allAmounts, null, 3.0);
            // Drinks
            ItemList sodaSizes = new ItemList(new Item[] { sodaSmallSize, sodaMedSize, sodaLargeSize });
            ItemList sodaOptions = new ItemList(new Item[] { cokeOption, pepsiOption, spriteOption });
            soda = new Food("Soft Drink", "Images/soda.jpg", sodaSizes, allAmounts, sodaOptions);
            ItemList bobaSizes = new ItemList(new Item[] { bobaSmallSize, bobaMedSize, bobaLargeSize });
            ItemList bobaOptions = new ItemList(new Item[] { bobaOption });
            boba = new Food("Boba Tea", "Images/boba.bmp", bobaSizes, allAmounts, bobaOptions);
            water = new Food("Water", "Images/water.jpg", null, allAmounts, null);
            // Categories
            meals = new Category("Meals", "Images/sandwich.jpg", new ItemList(new Item[] {burger, chicksandwich, bigsalad}));
            sides = new Category("Sides", "Images/french_fries.png", new ItemList(new Item[] {fries, sidesalad, fruit}));
            drinks = new Category("Drinks", "Images/soda.jpg", new ItemList(new Item[] {soda, boba, water}));

            // The top menu containing the food categories.
            menu = new ItemList(new Item[] { meals, sides, drinks });

            // Set up the stackpanel rows
            catCol = new Image[] { cat1, cat2, cat3 };
            amountCol = new Image[] {amount1, amount2, amount3};
            itemCol = new Image[] {item1, item2, item3};
            sizeCol = new Label[] {size1, size2, size3};
            optionsCol = new Label[] {option1, option2, option3};

            // Display logic
            menu.Display(catCol); // display categories in first row
            Category selectedCategory = (Category)menu.ReturnSingle();
            selectedCategory.DisplayFoods(itemCol); // display selected category's food in item row
            currentName.Content = menu.ReturnSingle().Name(); // display name of category
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            switch (currentCol)
            {
                case CATEGORY: updownCategory(true); break;
                case ITEM: updownItem(true); break;
                case SIZE: updownSize(true); break;
                case AMOUNT: updownAmount(true); break;
                case OPTIONS: updownOptions(true); break;
            }
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            switch (currentCol)
            {
                case CATEGORY: updownCategory(false); break;
                case ITEM: updownItem(false); break;
                case SIZE: updownSize(false); break;
                case AMOUNT: updownAmount(false); break;
                case OPTIONS: updownOptions(false); break;
            }
        }

        private void updownCategory(bool up)
        {
            if (up) { menu.Up(); }
            else { menu.Down(); }
            menu.Display(catCol); // display categories in first row
            Category category = ((Category)menu.ReturnSingle()); // get selected category
            category.DisplayFoods(itemCol); // display category's foods in second row
            currentName.Content = category.Name(); // display name of category's selected food
        }

        private void updownItem(bool up)
        {
            Category category = ((Category)menu.ReturnSingle());
            if (up) { category.FoodUp(); } // move food selection up
            else { category.FoodDown(); }
            category.DisplayFoods(itemCol); // display foods in second row
            if (category.Food().Sizes() != null)
            {
                category.Food().DisplaySizes(sizeCol);
            }
            else
            {
                clearSizeCol();
            }
            currentName.Content = category.FoodName(); // display name of food
            current.Source = new BitmapImage(new Uri(category.Food().Image(), UriKind.Relative));
        }

        private void updownSize(bool up)
        {
            Category category = ((Category)menu.ReturnSingle());
            Food food = category.Food();
            if (up) { food.SizeUp(); }
            else { food.SizeDown(); }
            if (food.Amounts() != null)
            {
                food.DisplayAmounts(amountCol);
            }
            // update current
        }

        private void updownAmount(bool up)
        {
            Category category = ((Category)menu.ReturnSingle());
            Food food = category.Food();
            if (up) { food.AmountUp(); }
            else { food.AmountDown(); }
            if (food.Options() != null)
            {
                food.DisplayOptions(optionsCol);
            }
            else { clearOptionsCol(); }
            // update current
        }

        private void updownOptions(bool up)
        {
            Category category = ((Category)menu.ReturnSingle());
            Food food = category.Food();
            if (up) { food.OptionUp(); }
            else { food.OptionDown(); }
            // update current
        }

        private void clearSizeCol()
        {
            foreach (Label l in sizeCol)
            {
                l.Content = "";
            }
        }
        private void clearOptionsCol()
        {
            foreach (Label l in sizeCol)
            {
                l.Content = "";
            }
        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            if (currentCol == CATEGORY) { return; }
            currentCol = 0; // current menu = categories
            currentName.Content = ((Category)menu.ReturnSingle()).Name(); // display name of category
        }

        private void right_Click(object sender, RoutedEventArgs e)
        {
            currentCol = 1; // current menu = foods
            currentName.Content = ((Category)menu.ReturnSingle()).FoodName(); // display name of category's selected food
        }
    }
}
