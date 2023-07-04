using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes;
        private List<Recipe> filteredRecipes;

        public MainWindow()
        {
            InitializeComponent();
            recipes = new List<Recipe>();
            filteredRecipes = new List<Recipe>();
        }

        private void addIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox ingredientNameTextBox = new TextBox();
            TextBox caloriesTextBox = new TextBox();
            TextBox foodGroupTextBox = new TextBox();

            StackPanel ingredientPanel = new StackPanel();
            ingredientPanel.Children.Add(new TextBlock() { Text = "Ingredient Name:" });
            ingredientPanel.Children.Add(ingredientNameTextBox);
            ingredientPanel.Children.Add(new TextBlock() { Text = "Calories:" });
            ingredientPanel.Children.Add(caloriesTextBox);
            ingredientPanel.Children.Add(new TextBlock() { Text = "Food Group:" });
            ingredientPanel.Children.Add(foodGroupTextBox);

            ingredientStackPanel.Children.Add(ingredientPanel);
        }

        private void addRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = recipeNameTextBox.Text;
            List<Ingredient> ingredients = new List<Ingredient>();

            foreach (StackPanel ingredientPanel in ingredientStackPanel.Children)
            {
                TextBox ingredientNameTextBox = (TextBox)ingredientPanel.Children[1];
                TextBox caloriesTextBox = (TextBox)ingredientPanel.Children[3];
                TextBox foodGroupTextBox = (TextBox)ingredientPanel.Children[5];

                string ingredientName = ingredientNameTextBox.Text;
                int calories = int.Parse(caloriesTextBox.Text);
                string foodGroup = foodGroupTextBox.Text;

                ingredients.Add(new Ingredient(ingredientName, calories, foodGroup));
            }

            Recipe recipe = new Recipe(recipeName, ingredients);
            recipes.Add(recipe);
            RefreshRecipeListBox();
            ClearInputFields();
        }

        private void RefreshRecipeListBox()
        {
            filteredRecipes = recipes.ToList(); // Make a copy of all recipes

            // Apply filters
            string ingredientNameFilter = ingredientNameFilterTextBox.Text.Trim();
            string foodGroupFilter = foodGroupFilterComboBox.SelectedItem as string;
            int maxCaloriesFilter;
            bool isCaloriesFilterValid = int.TryParse(maxCaloriesFilterTextBox.Text, out maxCaloriesFilter);

            if (!string.IsNullOrEmpty(ingredientNameFilter))
                filteredRecipes = filteredRecipes.Where(r => r.Ingredients.Any(i => i.Name.Contains(ingredientNameFilter))).ToList();

            if (!string.IsNullOrEmpty(foodGroupFilter))
                filteredRecipes = filteredRecipes.Where(r => r.Ingredients.Any(i => i.FoodGroup == foodGroupFilter)).ToList();

            if (isCaloriesFilterValid)
                filteredRecipes = filteredRecipes.Where(r => r.Ingredients.Sum(i => i.Calories) <= maxCaloriesFilter).ToList();

            allRecipesListBox.ItemsSource = filteredRecipes;
        }

        private void recipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Recipe selectedRecipe = allRecipesListBox.SelectedItem as Recipe;

            if (selectedRecipe != null)
            {
                int totalCalories = selectedRecipe.Ingredients.Sum(i => i.Calories);

                if (totalCalories > 300)
                {
                    MessageBox.Show("Warning: Total calories exceed 300!");
                }

                MessageBox.Show($"Total Calories: {totalCalories}");
            }
        }

        private void addStepButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox stepTextBox = new TextBox();
            stepTextBox.Margin = new Thickness(0, 5, 0, 0);
            stepStackPanel.Children.Add(stepTextBox);
        }

        private void ClearInputFields()
        {
            recipeNameTextBox.Text = string.Empty;
            ingredientStackPanel.Children.Clear();
            stepStackPanel.Children.Clear();
        }

        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshRecipeListBox();
        }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public Recipe(string name, List<Ingredient> ingredients)
        {
            Name = name;
            Ingredients = ingredients;
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }

        public Ingredient(string name, int calories, string foodGroup)
        {
            Name = name;
            Calories = calories;
            FoodGroup = foodGroup;
        }
    }
}
