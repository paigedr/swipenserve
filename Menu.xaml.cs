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
using System.Collections;
using Microsoft.Kinect;
using Coding4Fun.Kinect.Wpf; 
using System.Windows.Threading;

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
        public const int ORDER = 5;
        private const int SwipeDelay = 2;
        private const double swipeDistance = 0.3;
        private Rectangle lastBox;
        public Boolean canswipe = true;
        private DispatcherTimer SwipeTimer = new DispatcherTimer();

        public Menu()
        {
            InitializeComponent();
        }

        bool closing = false;
        const int skeletonCount = 6;
        Skeleton[] allSkeletons = new Skeleton[skeletonCount];
        
        protected bool makingLeftHandSwipe;

        protected double startingLeftHandX;
        protected double startingLeftHandY;
        protected double lastLeftHandX;
        protected double lastLeftHandY;

        void kinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor old = (KinectSensor)e.OldValue;

            StopKinect(old);

            KinectSensor sensor = (KinectSensor)e.NewValue;

            if (sensor == null)
            {
                return;
            }

            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.1f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 0.2f,
                MaxDeviationRadius = 0.5f
            };
            sensor.SkeletonStream.Enable(parameters);

            sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);
            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            try
            {
                sensor.Start();
            }
            catch (System.IO.IOException)
            {
                kinectSensorChooser1.AppConflictOccurred();
            }
        }

        void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (closing)
            {
                return;
            }

            //Get a skeleton
            Skeleton first = GetFirstSkeleton(e);

            if (first == null)
            {
                return;
            }

            //ScalePosition(leftEllipse, first.Joints[JointType.HandLeft]);

            GetCameraPoint(first, e);
            
            //DetectLeftHandSwipe(first, e);
            
        }

        private int lsCount = 0;
        private int rsCount = 0;
        private int dsCount = 0;
        private int usCount = 0;
        public void BoxGesture(int X, int Y)
        {
            Rectangle currentBox = GetBox(X, Y);
            if (lastBox == centerBox)
            {
                if (currentBox == leftBox)
                {
                    lastBox = leftBox;
                    lsCount++;
                    test.Text = "Left Swipe " + lsCount.ToString();
                    left_Click(null, null);
                }
                if (currentBox == rightBox)
                {
                    lastBox = rightBox;
                    rsCount++;
                    test.Text = "Right Swipe " + rsCount.ToString();
                    right_Click(null, null);
                }
                if (currentBox == topBox)
                {
                    lastBox = topBox;
                    usCount++;
                    test.Text = "Up Swipe " + usCount.ToString();
                    up_Click(null, null);
                }
                if (currentBox == bottomBox)
                {
                    lastBox = bottomBox;
                    dsCount++;
                    test.Text = "Down Swipe " + dsCount.ToString();
                    down_Click(null, null);
                }
            }
            else
            {
                lastBox = currentBox;
            }
        }

        public Rectangle GetBox(int X, int Y)
        {
            Point p = new Point(X,Y);
            if (check(centerBox, p)){
                test.Text = "Center";
                return centerBox;}
            else if (check(topBox, p))
            {
                test.Text = "Top";
                return topBox;
            }
            else if (check(bottomBox, p))
                return bottomBox;
            else if (check(leftBox, p))
                return leftBox;
            else if (check(rightBox, p))
                return rightBox;
            else
                return null;
        }

       
        void DetectLeftHandSwipe(Skeleton first, AllFramesReadyEventArgs e)
        {
            if (makingLeftHandSwipe && first.Joints[JointType.HandLeft].Position.X <= lastLeftHandX)
            {
                if (startingLeftHandX - first.Joints[JointType.HandLeft].Position.X > swipeDistance)
                {
                    test.Text = "swipe left";
                    left_Click(null, null);

                    makingLeftHandSwipe = false;

                    canswipe = false;
                    SwipeTimer.Start();
                }
            }
            else if (first.Joints[JointType.HandLeft].Position.X < lastLeftHandX)
            {
                makingLeftHandSwipe = true;
                startingLeftHandX = first.Joints[JointType.HandLeft].Position.X;
                test.Text = "going left";
            }

            else if (makingLeftHandSwipe && first.Joints[JointType.HandLeft].Position.X >= lastLeftHandX)
            {
                if (first.Joints[JointType.HandLeft].Position.X - startingLeftHandX > swipeDistance)
                {
                    test.Text = "swipe right";
                    right_Click(null, null);

                    makingLeftHandSwipe = false;

                    canswipe = false;
                    SwipeTimer.Start();
                }
            }
            else if (first.Joints[JointType.HandLeft].Position.X > lastLeftHandX)
            {
                makingLeftHandSwipe = true;
                startingLeftHandX = first.Joints[JointType.HandLeft].Position.X;
                test.Text = "going right";
            }

            if (makingLeftHandSwipe && first.Joints[JointType.HandLeft].Position.Y <= lastLeftHandY)
            {
                if (startingLeftHandY - first.Joints[JointType.HandLeft].Position.Y > swipeDistance)
                {
                    test.Text = "swipe down";
                    down_Click(null, null);

                    makingLeftHandSwipe = false;

                    canswipe = false;
                    SwipeTimer.Start();
                }
            }
            else if (first.Joints[JointType.HandLeft].Position.Y < lastLeftHandY)
            {
                makingLeftHandSwipe = true;
                startingLeftHandY = first.Joints[JointType.HandLeft].Position.Y;
                test.Text = "going down";
            }

            else if (makingLeftHandSwipe && first.Joints[JointType.HandLeft].Position.Y >= lastLeftHandY)
            {
                if (first.Joints[JointType.HandLeft].Position.Y - startingLeftHandY > swipeDistance)
                {
                    test.Text = "swipe up";
                    up_Click(null, null);    

                    makingLeftHandSwipe = false;

                    canswipe = false;
                    SwipeTimer.Start();
                }
            }
            else if (first.Joints[JointType.HandLeft].Position.Y > lastLeftHandY)
            {
                makingLeftHandSwipe = true;
                startingLeftHandY = first.Joints[JointType.HandLeft].Position.Y;
                test.Text = "going uppy";
            }
            else if (makingLeftHandSwipe)
            {
                makingLeftHandSwipe = false;
            }

            lastLeftHandX = first.Joints[JointType.HandLeft].Position.X;
            lastLeftHandY = first.Joints[JointType.HandLeft].Position.Y;
        }

      

        void GetCameraPoint(Skeleton first, AllFramesReadyEventArgs e)
        {

            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null ||
                    kinectSensorChooser1.Kinect == null)
                {
                    return;
                }


                //Map a joint location to a point on the depth map
                //left hand
                DepthImagePoint leftDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandLeft].Position);
                //right hand
                DepthImagePoint rightDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.HandRight].Position);
                DepthImagePoint headDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.Head].Position);
                DepthImagePoint shoulderDepthPoint = 
                    depth.MapFromSkeletonPoint(first.Joints[JointType.ShoulderRight].Position);

                //Map a depth point to a point on the color image
                //left hand
                ColorImagePoint leftColorPoint =
                    depth.MapToColorImagePoint(leftDepthPoint.X, leftDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right hand
                ColorImagePoint rightColorPoint =
                    depth.MapToColorImagePoint(rightDepthPoint.X, rightDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right shoukder
                ColorImagePoint shoulderColorPoint =
                    depth.MapToColorImagePoint(shoulderDepthPoint.X, shoulderDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);


                //Set location
                double ccLeft = shoulderColorPoint.X - InnerCanvas.Width / 2;
                double ccTop = shoulderColorPoint.Y - InnerCanvas.Height / 2;
                Canvas.SetLeft(ControlCanvas, ccLeft);
                Canvas.SetTop(ControlCanvas, ccTop);

                double eLeft = leftColorPoint.X - leftEllipse.Width / 2 + ccLeft - 150;
                double eTop = leftColorPoint.Y - leftEllipse.Height / 2 + ccTop - 150;
                //test.Text = "(" + ccLeft.ToString() + ", " + ccTop.ToString() + ")" + "(" + eLeft.ToString() + ", " + eTop.ToString() + ")";
                Canvas.SetLeft(leftEllipse, eLeft);
                Canvas.SetTop(leftEllipse, eTop);
                //CameraPosition(ControlCanvas, shoulderColorPoint);
                //CameraPosition(rightEllipse, rightColorPoint);
                BoxGesture(leftColorPoint.X, leftColorPoint.Y);
            }
        }


        Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }


                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get the first tracked skeleton
                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();

                return first;

            }
        }

        private void StopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                if (sensor.IsRunning)
                {
                    //stop sensor 
                    sensor.Stop();

                    //stop audio if not null
                    if (sensor.AudioSource != null)
                    {
                        sensor.AudioSource.Stop();
                    }


                }
            }
        }

        private void CameraPosition(FrameworkElement element, ColorImagePoint point)
        {
            //Divide by 2 for width and height so point is right in the middle 
            // instead of in top/left corner
            Canvas.SetLeft(element, point.X - element.Width / 2);
            Canvas.SetTop(element, point.Y - element.Height / 2);

        }

        private void ScalePosition(FrameworkElement element, Joint joint)
        {
            //convert the value to X/Y
            //Joint scaledJoint = joint.ScaleTo(1280, 720); 

            //convert & scale (.3 = means 1/3 of joint distance)
            Joint scaledJoint = joint.ScaleTo(1280, 720, .3f, .3f);

            Canvas.SetLeft(element, scaledJoint.Position.X);
            Canvas.SetTop(element, scaledJoint.Position.Y);

        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            StopKinect(kinectSensorChooser1.Kinect);
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
        ItemList order;
        Category meals, sides, drinks;
        Image[] catCol, itemCol;
        Label[] sizeCol, optionsCol, amountCol;
        int currentCol;
        SolidColorBrush selectedBackground = new SolidColorBrush(Colors.Cornsilk);
        SolidColorBrush defaultBackground = new SolidColorBrush(Colors.White);
        int orderRowIndex; //kind of a hacky way to keep track of where we are in the order column
        bool editing;
        String editString = "<-- Edit  ";
        String deleteString = "  --> Delete";

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
            test.Text = "CAKE";
            SwipeTimer.Tick += new EventHandler(ResetSwipe);
            SwipeTimer.Interval = TimeSpan.FromSeconds(SwipeDelay);

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
            // The list containing items in the order
            order = new ItemList();

            // Set up the stackpanel rows
            catCol = new Image[] { cat1, cat2, cat3 };
            amountCol = new Label[] {amount1, amount2, amount3};
            itemCol = new Image[] {item1, item2, item3};
            sizeCol = new Label[] {size1, size2, size3};
            optionsCol = new Label[] {option1, option2, option3};

            // Display logic
            editing = false;
            Category.Background = selectedBackground;
            menu.Display(catCol); // display categories in first row
            Category selectedCategory = (Category)menu.ReturnSingle();
            selectedCategory.DisplayFoods(itemCol); // display selected category's food in item row
            hideOrderOptions();
        }

        private void updatePrice()
        {
            Category category = (Category)menu.ReturnSingle();
            Food food = category.Food();
            if (food == null || food.TotalPrice() == 0)
            {
                currentPrice.Content = "";
            }
            else { currentPrice.Content = "$" + food.TotalPrice().ToString("N2"); }
        }

        // EVENT HANDLERS
        private void up_Click(object sender, RoutedEventArgs e)
        {
            switch (currentCol)
            {
                case CATEGORY: updownCategory(true); break;
                case ITEM: updownItem(true); break;
                case SIZE: updownSize(true); break;
                case AMOUNT: updownAmount(true); break;
                case OPTIONS: updownOptions(true); break;
                case ORDER: updownOrderCol(true); break;
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
                case ORDER: updownOrderCol(false); break;
            }
            if (currentCol != CATEGORY) { updatePrice(); }
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
                case ORDER: handleOrderLeftClick(); break;
            }
            if (currentCol != CATEGORY) { updatePrice(); }
        }

        private void right_Click(object sender, RoutedEventArgs e)
        {
            switch (currentCol)
            {
                case CATEGORY: selectItemCol(true); break;
                case ITEM: selectSizeCol(true); break;
                case SIZE: selectAmountCol(true); break;
                case AMOUNT: selectOptionsCol(true); break;
                case OPTIONS: addToOrder(); selectOrderCol(); break;
                case ORDER: handleOrderRightClick(); break;
            }
            if (currentCol != CATEGORY) { updatePrice(); }
        }

        // Helper event handlers

        private void handleOrderRightClick()
        {
            Label l = (Label)Order.Children[orderRowIndex];
            if (l == newItemLabel)
            {
                // go back to start
                selectCatCol(false);
            }
            else if (l == checkOutLabel)
            {
                // exit menu screen, show checkout
                main.Visibility = System.Windows.Visibility.Hidden;
                Canvas parent = (Canvas)this.Parent;
                Label label1 = new Label();
                label1.Content = "Thanks for using Swipe n' Serve!\nYour order is waiting at the pick-up window.\nSee you again soon!";
                label1.FontSize = 25;
                parent.Children.Add(label1);
                parent.Children[0].Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                // delete item
                order.Delete(order.Item(order.SelectedIndex()));
                Order.Children.RemoveAt(orderRowIndex);
                orderRowIndex--;
                order.Up();
                showSelectedOrderItem();
                updateTotalPrice();
                if (order.Count() == 0)
                {
                    // don't allow user to check out
                    Order.Children.Remove(checkOutLabel);
                }
            }
        }

        private void handleOrderLeftClick()
        {
            Label l = (Label)Order.Children[orderRowIndex];
            if (l.Tag != null && l.Tag == "Item")
            {
                l.Content = l.Content.ToString() + " EDITING";
                l.Background = new SolidColorBrush(Colors.LightPink);
                editing = true;
                selectOptionsCol(false);
            }
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

        private void updownOrderCol(bool up)
        {
            clearOrderSelections();
            if (orderRowIndex > 1 && orderRowIndex < order.Count())
            {
                if (up) { order.Up(); }
                else { order.Down(); }
            }
            if (up) { orderRowIndex--; if (orderRowIndex < 0) orderRowIndex = 0; }
            else { orderRowIndex++; if (orderRowIndex > order.Count() + 2) orderRowIndex--; }
            showSelectedOrderItem();
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

        private void hideOrderOptions()
        {
            Order.Children.Remove(newItemLabel);
            Order.Children.Remove(checkOutLabel);
        }

        private void unhideOrderOptions()
        {
            Order.Children.Insert(1, newItemLabel);
            Order.Children.Insert(2 + order.Count(), checkOutLabel);
        }

        private void clearOrderSelections()
        {
            foreach (UIElement e in Order.Children) {
                Label l = new Label() ;
                if (e.GetType() == test.GetType())
                {
                    l.Content = ((TextBlock)e).Text;
                }
                else
                {
                    l = (Label)e;
                }
                if (l.Tag != null && l.Tag.Equals("Item"))
                {
                    if (!editing)
                    {
                        l.Background = defaultBackground;
                    }
                }
                else if (l.Tag != null && l.Tag.Equals("Option"))
                {
                    l.Background = new SolidColorBrush(Colors.LightGreen);
                    l.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void selectCatCol(bool fromLeft)
        {
            clearSizeCol();
            clearAmountCol();
            clearOptionsCol();
            clearOrderSelections();
            hideOrderOptions();
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
            hideOrderOptions();
            clearOrderSelections();
            Options.Background = selectedBackground;
            Amount.Background = defaultBackground;
            currentCol = OPTIONS;
            Category category = ((Category)menu.ReturnSingle());
            Food food = category.Food();
            if (food.Options() == null) { selectAmountCol(false); }
            // update current
        }

        private void selectOrderCol()
        {
            editing = false;
            unhideOrderOptions();
            currentCol = ORDER;
            orderRowIndex = 1;
            clearOrderSelections();
            Options.Background = defaultBackground;
            showSelectedOrderItem();
        }
        // Order-related methods

        private void showSelectedOrderItem()
        {
            Label selectedItem = new Label();
            if (Order.Children[orderRowIndex].GetType() == selectedItem.GetType())
            {
                selectedItem = (Label)Order.Children[orderRowIndex];
            }
            else
            {
                selectedItem.Content = ((TextBox)Order.Children[orderRowIndex]).Text;
            }
            if (selectedItem.Tag != null && selectedItem.Tag == "Item")
            {
                selectedItem.Background = selectedBackground;
            }
            else
            {
                selectedItem.Background = new SolidColorBrush(Colors.Green);
                selectedItem.Foreground = new SolidColorBrush(Colors.White);
            }

            if (orderRowIndex > 1 && orderRowIndex < (order.Count() + 2))
                test.Text = "Item Selected";
            else
                test.Text = orderRowIndex.ToString();
        }

        private void addNewItem()
        {
            selectCatCol(false);
        }

        private Food currentItem()
        {
            Category category = ((Category)menu.ReturnSingle());
            Food food = category.Food();
            ItemList itemSize = null;
            ItemList itemOptions = null;
            if (food.Sizes() != null) { 
                itemSize = new ItemList(new Item[] { food.Sizes().ReturnSingle() });
            }
            ItemList itemAmount = new ItemList(new Item[] { food.Amounts().ReturnSingle() });
            if (food.Options() != null) { itemOptions = new ItemList(new Item[] { food.Options().ReturnSingle() }); } //could be multiple in the future
            return new Food(food.Name(), food.Image(), itemSize, itemAmount, itemOptions, food.TotalPrice());
        }

        private void addToOrder()
        {
            if (editing)
            {
                Order.Children.Remove(Order.Children[orderRowIndex-1]);
                order.Delete(order.Item(order.SelectedIndex()));
            }
            order.Add(currentItem());
            String s = formatOrderItem((Food)order.Item(order.Count() - 1));
            Label l = new Label();
            l.Content = s;
            l.Tag = "Item";
            Order.Children.Insert(1, l);
            Order.UpdateLayout();
            updateTotalPrice();
        }

        private void updateTotalPrice()
        {
            totalPrice.Content = "$" + order.TotalPrice().ToString("N2");
        }

        private String formatOrderItem(Food item)
        {
            String amount = item.Amounts().ReturnSingle().Name();
            String name = item.Name();
            if (item.Amounts().ReturnSingle().Price() > 1) {
                name += "s"; //plural
            }
            String option = item.Options().ReturnSingle().Name();
            String size = item.SizeName();
            String price = "$" + item.Price().ToString("N2");
            if (option.ToLower().Equals("none"))
            {
                return amount + " " + size + " " + name + " " + ": " + price;
            }
            else {
                return amount + " " + size + " " + name + " " + "with " + option + ": " + price;
            }
        }

        private void ResetSwipe(Object sender, EventArgs args)
        {
            canswipe = true;
            SwipeTimer.Stop();
        }

        private bool check(Rectangle element, Point point)
        {
                bool vert_in = (point.Y >= Canvas.GetTop(element) + Canvas.GetTop(ControlCanvas)) && (point.Y <= Canvas.GetTop(element) + Canvas.GetTop(ControlCanvas) + element.ActualHeight);
                bool hori_in = (point.X >= Canvas.GetLeft(element) + Canvas.GetLeft(ControlCanvas)) && (point.X <= Canvas.GetLeft(element) + Canvas.GetLeft(ControlCanvas) + element.ActualWidth);
                if (vert_in && hori_in)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }
}
