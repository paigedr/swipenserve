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
        Image[] catCol, itemCol;
        Label[] sizeCol, optionsCol, amountCol;
        int currentCol;
        SolidColorBrush selectedBackground = new SolidColorBrush(Colors.Cornsilk);
        SolidColorBrush defaultBackground = new SolidColorBrush(Colors.White);

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Amounts
            Item one = new Item("1", "Images/number1.jpg", 1.0);
            Item two = new Item("2", "Images/number2.jpg", 2.0);
            Item three = new Item("3", "Images/number3.jpg", 3.0);
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
            Item noneOption = new Item("None", null, 0.0);
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
            ItemList burgerOptions = new ItemList(new Item[] { noneOption, cheeseOption, picklesOption, onionsOption });
            burger = new Food("Hamburger", "Images/hamburger.jpg", burgerSizes, allAmounts, burgerOptions); //base price needed?
            ItemList sandwichSizes = new ItemList(new Item[] { sandwichRegSize, sandwichJumboSize });
            chicksandwich = new Food("Chicken Sandwich", "Images/chicken_sandwich.jpg", sandwichSizes, allAmounts, burgerOptions);
            ItemList saladOptions = new ItemList(new Item[] { noneOption, cheeseOption, italianOption, ranchOption });
            bigsalad = new Food("Big Salad", "Images/big_salad.jpg", null, allAmounts, saladOptions, 5.0);
            // Sides
            ItemList friesSizes = new ItemList(new Item[] { friesSmallSize, friesMedSize, friesLargeSize });
            ItemList friesOptions = new ItemList(new Item[] { noneOption, chiliCheeseOption });
            fries = new Food("French Fries", "Images/french_fries.png", friesSizes, allAmounts, friesOptions);
            sidesalad = new Food("Side Salad", "Images/side_salad.png", null, allAmounts, saladOptions, 2.5);
            fruit = new Food("Fruit Salad", "Images/fruit_cup.png", null, allAmounts, null, 3.0);
            // Drinks
            ItemList sodaSizes = new ItemList(new Item[] { sodaSmallSize, sodaMedSize, sodaLargeSize });
            ItemList sodaOptions = new ItemList(new Item[] { cokeOption, pepsiOption, spriteOption });
            soda = new Food("Soft Drink", "Images/soda.jpg", sodaSizes, allAmounts, sodaOptions);
            ItemList bobaSizes = new ItemList(new Item[] { bobaSmallSize, bobaMedSize, bobaLargeSize });
            ItemList bobaOptions = new ItemList(new Item[] { noneOption, bobaOption });
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
            amountCol = new Label[] {amount1, amount2, amount3};
            itemCol = new Image[] {item1, item2, item3};
            sizeCol = new Label[] {size1, size2, size3};
            optionsCol = new Label[] {option1, option2, option3};

            // Display logic
            Category.Background = selectedBackground;
            menu.Display(catCol); // display categories in first row
            Category selectedCategory = (Category)menu.ReturnSingle();
            selectedCategory.DisplayFoods(itemCol); // display selected category's food in item row
        }

        private void updatePrice()
        {
            Category category = (Category)menu.ReturnSingle();
            Food food = category.Food();
            if (food == null || food.TotalPrice() == 0)
            {
                currentPrice.Content = "";
            }
            else { currentPrice.Content = food.TotalPrice(); }
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
            if (currentCol != CATEGORY) { updatePrice(); }
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
            if (currentCol != CATEGORY) { updatePrice(); }
        }

        private void updownCategory(bool up)
        {
            if (up) { menu.Up(); }
            else { menu.Down(); }
            menu.Display(catCol); // display categories in first row
            Category category = ((Category)menu.ReturnSingle()); // get selected category
            category.DisplayFoods(itemCol); // display category's foods in second row
        }

        private void updownItem(bool up)
        {
            Category category = ((Category)menu.ReturnSingle());
            if (up) { category.FoodUp(); } // move food selection up
            else { category.FoodDown(); }
            category.DisplayFoods(itemCol); // display foods in second row
            if (category.Food().Sizes() != null)
            {
                clearAmountCol();
                category.Food().DisplaySizes(sizeCol);
            }
            else
            {
                clearSizeCol();
                category.Food().DisplayAmounts(amountCol);
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
            // refresh display
            food.DisplaySizes(sizeCol);
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
            // refresh display
            food.DisplayAmounts(amountCol);
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
            // refresh display
            food.DisplayOptions(optionsCol);
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
            foreach (Label l in optionsCol)
            {
                l.Content = "";
            }
        }

        private void clearAmountCol()
        {
            foreach (Label l in amountCol)
            {
                l.Content = "";
            }
        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            switch (currentCol)
            {
                case CATEGORY: break;
                case ITEM: selectCatCol(false); break;
                case SIZE: selectItemCol(false); break;
                case AMOUNT: selectSizeCol(false); break;
                case OPTIONS: selectAmountCol(false); break;
            }
            if (currentCol != CATEGORY) { updatePrice(); }
        }

        private void right_Click(object sender, RoutedEventArgs e)
        {
             switch (currentCol) {
                 case CATEGORY: selectItemCol(true); break;
                 case ITEM: selectSizeCol(true); break;
                 case SIZE: selectAmountCol(true); break;
                 case AMOUNT: selectOptionsCol(true); break;
                 case OPTIONS: break;
            }
             if (currentCol != CATEGORY) { updatePrice();  }
        }

        private void selectCatCol(bool fromLeft)
        {
            clearSizeCol();
            clearAmountCol();
            Category.Background = selectedBackground;
            Item.Background = defaultBackground;
            currentCol = CATEGORY;
            Category category = ((Category)menu.ReturnSingle());
            currentName.Content = "";
            current.Source = new BitmapImage(new Uri("", UriKind.Relative)); //blank image?
            currentPrice.Content = ""; // don't show price in this column.
        }

        private void selectItemCol(bool fromLeft)
        {
            clearAmountCol();
            clearOptionsCol();
            Item.Background = selectedBackground;
            if (fromLeft) { Category.Background = defaultBackground; }
            else { Size.Background = defaultBackground; }
            currentCol = ITEM;
            Category category = ((Category)menu.ReturnSingle());
            if (category.Food().Sizes() != null)
            {
                category.Food().DisplaySizes(sizeCol);
            }
            else
            {
                // if there are no sizes, display the next column (amounts)
                category.Food().DisplayAmounts(amountCol);
            }
            currentName.Content = category.FoodName(); // display name of category's selected food
            current.Source = new BitmapImage(new Uri(category.Food().Image(), UriKind.Relative));
        }

        private void selectSizeCol(bool fromLeft)
        {
            clearOptionsCol();
            clearAmountCol();
            Size.Background = selectedBackground;
            if (fromLeft) { Item.Background = defaultBackground; }
            else { Amount.Background = defaultBackground; }
            currentCol = SIZE;
            Category category = ((Category)menu.ReturnSingle());
            Food food = category.Food();
            if (food.Amounts() != null)
            {
                food.DisplayAmounts(amountCol);
            }
            if (food.Sizes() == null) 
            { 
                if (fromLeft) { selectAmountCol(true); }
                else { selectItemCol(false); }
            }
            // update current
        }

        private void selectAmountCol(bool fromLeft)
        {
            clearOptionsCol();
            Amount.Background = selectedBackground;
            if (fromLeft) { Size.Background = defaultBackground; }
            else { Options.Background = defaultBackground; }
            currentCol = AMOUNT;
            Category category = ((Category)menu.ReturnSingle());
            Food food = category.Food();
            if (food.Options() != null)
            {
                food.DisplayOptions(optionsCol);
            }
            //update current
        }

        private void selectOptionsCol(bool fromLeft)
        {
            Options.Background = selectedBackground;
            Amount.Background = defaultBackground;
            currentCol = OPTIONS;
            Category category = ((Category)menu.ReturnSingle());
            Food food = category.Food();
            if (food.Options() == null) { selectAmountCol(false); }
            // update current
        }
    }
}
