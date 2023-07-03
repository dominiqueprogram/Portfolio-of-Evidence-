using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;   

namespace RecipeApp
{
    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }
    }

    public class Step
    {
        public string Description { get; set; }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            Steps = new List<Step>();
        }

        public void Display()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Recipe: {Name}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ingredients:\n");

            Console.ForegroundColor = ConsoleColor.White;
            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} {ingredient.Name}");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nSteps:\n");

            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < Steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Steps[i].Description}");
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("***************");
        }
		


			public int CalculateTotalCalories()
		{
			int totalCalories = 0;

			foreach (var ingredient in Ingredients)
			{
				double ingredientCalories = ingredient.Calories * ingredient.Quantity;
				totalCalories += (int)Math.Round(ingredientCalories);
			}

			return totalCalories;
		}

		
    }

    class Program
    {
        public delegate void RecipeCaloriesExceededHandler(Recipe recipe);

        static List<Recipe> recipes = new List<Recipe>();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("*** Welcome to Recipe App ***");
            Console.WriteLine();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Select an option:");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. Add a recipe");
                Console.WriteLine("2. Display all recipes");
                Console.WriteLine("3. Select a recipe to display");
                Console.WriteLine("4. Exit");

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("****************************");
                Console.WriteLine();

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a number corresponding to the menu options.");
                    Console.WriteLine();
                }

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Selected option: " + choice);
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        AddRecipe();
                        break;

                    case 2:
                        DisplayAllRecipes();
                        break;

                    case 3:
                        SelectRecipeToDisplay();
                        break;

                    case 4:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Thank you for using Recipe App!");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        static void AddRecipe()
   {
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("*** Add Recipe ***");
    Console.WriteLine();

    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("Enter the number of recipes to add: ");
    int numRecipes;

    while (!int.TryParse(Console.ReadLine(), out numRecipes) || numRecipes <= 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("Invalid input. Please enter a positive integer for the number of recipes: ");
    }

    Console.WriteLine();

    for (int recipeIndex = 1; recipeIndex <= numRecipes; recipeIndex++)
    {
        Recipe recipe = new Recipe();

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"*** Recipe {recipeIndex} ***");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter the name of the recipe: ");
        string recipeName = Console.ReadLine();
        recipe.Name = recipeName;

        Console.Write("Enter the number of ingredients: ");
        int numIngredients;

        while (!int.TryParse(Console.ReadLine(), out numIngredients) || numIngredients <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. Please enter a positive integer for the number of ingredients: ");
        }

        Console.WriteLine();

        for (int ingredientIndex = 1; ingredientIndex <= numIngredients; ingredientIndex++)
        {
            Ingredient ingredient = new Ingredient();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Enter the name of ingredient {ingredientIndex}: ");
            string ingredientName = Console.ReadLine();
            ingredient.Name = ingredientName;

            double quantity;
            Console.Write($"Enter the quantity of ingredient {ingredientIndex} (in decimal form): ");

            while (!double.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid input. Please enter a positive number for the quantity: ");
            }

            ingredient.Quantity = quantity;

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Enter the unit of measurement of ingredient {ingredientIndex}: ");
            ingredient.Unit = Console.ReadLine();

            Console.Write($"Enter the number of calories for ingredient {ingredientIndex}: ");
            int calories;

            while (!int.TryParse(Console.ReadLine(), out calories) || calories <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid input. Please enter a positive integer for the calories: ");
            }

            ingredient.Calories = calories;
             
			 Console.WriteLine();
			 
            Console.WriteLine("Select the food group that ingredient {0} belongs to:", ingredientIndex);
            Console.WriteLine("1. Starchy foods");
            Console.WriteLine("2. Vegetables and fruits");
            Console.WriteLine("3. Dry beans, peas, lentils, and soya");
            Console.WriteLine("4. Chicken, fish, meat, and eggs");
            Console.WriteLine("5. Milk and dairy products");
            Console.WriteLine("6. Fats and oil");
            Console.WriteLine("7. Water");

            int foodGroupOption;

            while (!int.TryParse(Console.ReadLine(), out foodGroupOption) || foodGroupOption < 1 || foodGroupOption > 7)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid input. Please enter a number between 1 and 7 to select the food group: ");
            }

            ingredient.FoodGroup = GetFoodGroupFromOption(foodGroupOption);

            Console.WriteLine();

            recipe.Ingredients.Add(ingredient);
        }

        Console.Write("Enter the number of steps: ");
        int numSteps;

        while (!int.TryParse(Console.ReadLine(), out numSteps) || numSteps <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Invalid input. Please enter a positive integer for the number of steps: ");
        }

        Console.WriteLine();

        for (int stepIndex = 1; stepIndex <= numSteps; stepIndex++)
        {
            Step step = new Step();

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Enter the description of step {stepIndex}: ");
            string stepDescription = Console.ReadLine();
            step.Description = stepDescription;

            Console.WriteLine();

            recipe.Steps.Add(step);
        }

        Console.WriteLine();

        recipes.Add(recipe);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Recipe {recipeIndex} added successfully!");

        Console.WriteLine();
       }
    }

		static string GetFoodGroupFromOption(int option)
		{
		switch (option)
		{
			case 1:
				return "Starchy foods";
			case 2:
				return "Vegetables and fruits";
			case 3:
				return "Dry beans, peas, lentils, and soya";
			case 4:
				return "Chicken, fish, meat, and eggs";
			case 5:
				return "Milk and dairy products";
			case 6:
				return "Fats and oil";
			case 7:
				return "Water";
				default:
				return string.Empty;
			}
		}

		
		
		
        static void DisplayAllRecipes()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("*** All Recipes ***");
            Console.WriteLine();

            if (recipes.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No recipes found.");
            }
            else
            {
                var sortedRecipes = recipes.OrderBy(recipe => recipe.Name);

                Console.ForegroundColor = ConsoleColor.White;
                foreach (var recipe in sortedRecipes)
                {
                    Console.WriteLine(recipe.Name);
                }
            }

            Console.WriteLine();
        }

					static void SelectRecipeToDisplay()
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine("*** Select Recipe ***");
			Console.WriteLine();

			if (recipes.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("No recipes found.");
				Console.WriteLine();
				return;
			}

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Select a recipe from the list:");
			Console.WriteLine();

			var sortedRecipes = recipes.OrderBy(recipe => recipe.Name);

			int index = 1;
			foreach (var recipe in sortedRecipes)
			{
				Console.WriteLine($"{index}. {recipe.Name}");
				index++;
			}

			Console.WriteLine();

			int selectedRecipeIndex;
			while (!int.TryParse(Console.ReadLine(), out selectedRecipeIndex) || selectedRecipeIndex < 1 || selectedRecipeIndex > recipes.Count)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid input. Please enter a valid recipe number.");
				Console.WriteLine();
			}

			Recipe selectedRecipe = sortedRecipes.ElementAt(selectedRecipeIndex - 1);

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine();
			Console.WriteLine($"Displaying Recipe: {selectedRecipe.Name}");
			Console.WriteLine();

			selectedRecipe.Display();

			int totalCalories = selectedRecipe.CalculateTotalCalories();
			Console.WriteLine();
			Console.WriteLine($"Total Calories: {totalCalories}");

			if (totalCalories > 300)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Warning: The total calories of this recipe exceed 300!");
			}

			Console.WriteLine();
		}


    }
}
