﻿using System;
using System.Diagnostics;
using Xamarin.Forms;
using Recipes.Views;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Recipes.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        public Command EditItemCommand { get; }

        string _itemId;
        string _recipeName;
        string _imageUrl;
        string _ingredients;
        string _recipeBody;
        FormattedString _recipeUrl;

        bool _recipeNameVisible;
        bool _imageUrlVisible;
        bool _ingredientsVisible;
        bool _recipeBodyVisible;
        bool _recipeUrlVisible;

        public string Id { get; set; }

        public ItemDetailViewModel()
        {
            EditItemCommand = new Command(OnEditItem);

            RecipeNameVisible = true;
            ImageUrlVisible = true;
            IngredientsVisible = true;
            RecipeBodyVisible = true;
            RecipeUrlVisible = true;
        }

		public string ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                if (value == null)
                    return;

                _itemId = value;
                LoadItemId(value);
            }
        }

        public string RecipeName
        {
            get => _recipeName;
            set => SetProperty(ref _recipeName, value);
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }


        IList<string> source;
        public ObservableCollection<string> _ingredientCheckList { get; private set; }

        //public string Ingredients
        //{
        //    get => _ingredients;
        //    set => SetProperty(ref _ingredients, value);
        //}
		public ObservableCollection<string> IngredientChecklist
		{
			get => _ingredientCheckList;
			set => SetProperty(ref _ingredientCheckList, value);
		}

		public string RecipeBody
        {
            get => _recipeBody;
            set => SetProperty(ref _recipeBody, value);
        }

        public FormattedString RecipeUrl
        {
            get => _recipeUrl;
            set => SetProperty(ref _recipeUrl, value);
        }

        public bool RecipeNameVisible
        {
            get => _recipeNameVisible;
            set => SetProperty(ref _recipeNameVisible, value);
        }

        public bool ImageUrlVisible
        {
            get => _imageUrlVisible;
            set => SetProperty(ref _imageUrlVisible, value);
        }

        public bool IngredientsVisible
        {
            get => _ingredientsVisible;
            set => SetProperty(ref _ingredientsVisible, value);
        }

        public bool RecipeBodyVisible
        {
            get => _recipeBodyVisible;
            set => SetProperty(ref _recipeBodyVisible, value);
        }

        public bool RecipeUrlVisible
        {
            get => _recipeUrlVisible;
            set => SetProperty(ref _recipeUrlVisible, value);
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                RecipeName = item.RecipeName;
                ImageUrl = item.ImageUrl;
                //Ingredients = item.Ingredients;
                RecipeBody = item.RecipeBody;
                RecipeUrl = item.RecipeUrl;

                source = new List<string>((item.Ingredients).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));
                IngredientCheckList = new ObservableCollection<string>(source);

                Title = RecipeName;

                var emptyFormattedString = new FormattedString();
                emptyFormattedString.Spans.Add(new Span { Text = "" });

                RecipeNameVisible = !String.IsNullOrEmpty(RecipeName);
                ImageUrlVisible = !String.IsNullOrEmpty(ImageUrl);
                IngredientsVisible = (IngredientCheckList.Count > 0);
                RecipeBodyVisible = !String.IsNullOrEmpty(RecipeBody);
                RecipeUrlVisible = !(RecipeUrl == null || FormattedString.Equals(RecipeUrl, emptyFormattedString));
                
            }
            catch (Exception) 
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private async void OnEditItem(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(EditItemPage)}?{nameof(EditItemViewModel.Id)}={_itemId}");
        }

        public void OnAppearing()
        {
            IsBusy = true;
            LoadItemId(_itemId);
        }
    }
}
