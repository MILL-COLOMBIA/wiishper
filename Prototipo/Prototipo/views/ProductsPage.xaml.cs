using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Plugin.Toasts;

namespace Prototipo
{
    public partial class ProductsPage : ContentPage
    {

        IToastNotificator notificator;
        CardStackView productCards;
        public ProductsPage()
        {
            InitializeComponent();
            BindingContext = new ProductViewModel();
            productCards = new CardStackView();
            productCards.SetBinding(CardStackView.ItemsSourceProperty, "ProductList");
            productCards.SwipedLeft += SwipedLeft;
            productCards.SwipedRight += SwipedRight;
            RelativeLayout view = new RelativeLayout();
            view.BackgroundColor = Color.FromHex("F2F2F2");
            NavigationPage.SetHasNavigationBar(this, false);
            notificator = DependencyService.Get<IToastNotificator>();

            #region UI_building
            view.Children.Add(productCards,
                              Constraint.Constant(30),
                              Constraint.Constant(60),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width - 60;
                              }),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Height - 160;
                              })
                );

            this.LayoutChanged += (object sender, System.EventArgs e) =>
            {
                productCards.CardMoveDistance = (int)(this.Width * 0.40f);
            };

            BoxView upperSeparator = new BoxView
            {
                BackgroundColor = Color.FromHex("86CACD")
            };

            view.Children.Add(upperSeparator,
                              Constraint.Constant(0),
                              Constraint.Constant(50),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width;
                              }),
                              Constraint.Constant(2));

            StackLayout upperBar = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.FromHex("5D5D5D")
            };

            Image logo = new Image
            {
                Source = ImageSource.FromFile("wordlogo.png"),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 50
            };

            upperBar.Children.Add(logo);

            view.Children.Add(upperBar,
                              Constraint.Constant(0),
                              Constraint.Constant(0),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width;
                              }),
                              Constraint.Constant(50));

            #region BottomMenu

            StackLayout bottomMenu = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.White
            };

            Image btnFriends = new Image
            {
                Source = "people_off.png",
                BackgroundColor = Color.White,
                HeightRequest = 25,
                WidthRequest = 25,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            TapGestureRecognizer tapper_friends = new TapGestureRecognizer();
            tapper_friends.Tapped += OnFriends;
            btnFriends.GestureRecognizers.Add(tapper_friends);

    //        Button friends = new Button
    //        {
    //            Image = "people_off.png",
				//BackgroundColor = Color.White,
    //            HorizontalOptions = LayoutOptions.CenterAndExpand,
    //            BorderRadius = 0,
    //            HeightRequest = 50
    //        };

    //        friends.Clicked += OnFriends;

    //        Button product_btn = new Button
    //        {
    //            Image = "main_on.png",
				//BackgroundColor = Color.White,
    //            HorizontalOptions = LayoutOptions.CenterAndExpand,
    //            BorderRadius = 0,
    //            HeightRequest = 60
    //        };

    //        product_btn.Clicked += OnProducts;

            Image btnProducts = new Image
            {
                Source = "main_on.png",
                BackgroundColor = Color.White,
                HeightRequest = 50,
                WidthRequest = 50,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

    //        Button profile = new Button
    //        {
    //            Image = "profile_off.png",
				//BackgroundColor = Color.White,
    //            HorizontalOptions = LayoutOptions.CenterAndExpand,
				//BorderRadius = 0,
    //            HeightRequest = 50
    //        };

            Image btnProfile = new Image
            {
                Source = "profile_off.png",
                BackgroundColor = Color.White,
                HeightRequest = 25,
                WidthRequest = 25,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            TapGestureRecognizer tapper_profile = new TapGestureRecognizer();
            tapper_profile.Tapped += OnProfile;
            btnProfile.GestureRecognizers.Add(tapper_profile);

            //profile.Clicked += OnProfile;

            bottomMenu.Children.Add(btnFriends);
            bottomMenu.Children.Add(btnProducts);
            bottomMenu.Children.Add(btnProfile);

            view.Children.Add(bottomMenu,
                              Constraint.Constant(0),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Height - 70;
                              }),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width;
                              }),
                              Constraint.Constant(65));

            //BoxView bottomSeparator = new BoxView
            //{
            //    BackgroundColor = Color.FromHex("80BCBEC0")
            //};

            //view.Children.Add(bottomSeparator,
            //                  Constraint.Constant(0),
            //                  Constraint.RelativeToParent((parent) =>
            //                  {
            //                      return parent.Height - Device.OnPlatform<double>(46, 55, 0);
            //                  }
            //                  ),
            //                  Constraint.RelativeToParent((parent) =>
            //                  {
            //                      return parent.Width;
            //                  }),
            //                  Constraint.Constant(2));
            #endregion

            Button like = new Button
            {
                Image = "like.png",
				BackgroundColor = Color.Transparent
            };

            like.Clicked += OnLike;

            Button dislike = new Button
            {
                Image = "dislike.png",
				BackgroundColor = Color.Transparent
            };

            dislike.Clicked += OnReject;

            view.Children.Add(like,
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width - 49;
                              }),
                              Constraint.RelativeToParent((parent) =>{return parent.Height - 93;}),
                              Constraint.Constant(33),
                              Constraint.Constant(33));

            view.Children.Add(dislike,
                              Constraint.Constant(16),
                              Constraint.RelativeToParent((parent) => { return parent.Height - 93; }),
                              Constraint.Constant(33),
                              Constraint.Constant(33));
            #endregion

            Content = view;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            notificator.Notify(ToastNotificationType.Info, "Wiishper", "Estamos cargando productos para tí", TimeSpan.FromSeconds(2));
            ((ProductViewModel)BindingContext).ProductList = await App.Manager.GetProducts();
            if (((ProductViewModel)BindingContext).ProductList == null || ((ProductViewModel)BindingContext).ProductList.Count() <= 0)
            {
                await notificator.Notify(ToastNotificationType.Error, "Wiishper", "Ooops, no encontramos ningún producto", TimeSpan.FromSeconds(2));
            }
        }

        private async void OnReject(object sender, EventArgs e)
        {
            int idproduct = productCards.product.idproducts;
            if (RestService.LoggedUser != null)
            {
                string result = await App.Manager.RejectProduct(idproduct);
                
                if (!result.Equals("FAIL"))
                {
                    await productCards.NextLeft();
                }
            }
            else
            {
                App.Database.SaveProduct(new Taste { idproducts = idproduct, inter_date = new DateTime(), liked = false });
                await productCards.NextLeft();
            }
            if (productCards.IsEnding)
            {
                List<Product> temp = await App.Manager.GetProducts();
                List<Product> actual = productCards.ItemsSource;
                productCards.ItemsSource.AddRange(temp.Where(x => !actual.Any(y => x.idproducts == y.idproducts)));
            }
        }

        private async void OnLike(object sender, EventArgs e)
        {
            int idproduct = productCards.product.idproducts;
            if (RestService.LoggedUser != null)
            {
                string result = await App.Manager.LikeProduct(idproduct);
                
                if (!result.Equals("FAIL"))
                {
                    await productCards.NextRight();
                }
            }
            else
            {
                App.Database.SaveProduct(new Taste { idproducts = idproduct, inter_date = new DateTime(), liked = true });
                await productCards.NextRight();
            }
            if (productCards.IsEnding)
            {
                List<Product> temp = await App.Manager.GetProducts();
                List<Product> actual = productCards.ItemsSource;
                productCards.ItemsSource.AddRange(temp.Where(x => !actual.Any(y => x.idproducts == y.idproducts)));
            }
        }

        private async void OnNewsfeed(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new NotificationsPage(), this);
            }
            await Navigation.PopAsync(false);
        }

        private async void OnFriends(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new FriendsPage(), this);
            }
            await Navigation.PopAsync(false);
        }

        private async void OnProducts(object sender, EventArgs e)
        {

        }

        private async void OnActivity(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new ActivityPage(), this);
            }
            await Navigation.PopAsync(false);
        }

        private async void OnProfile(object sender, EventArgs e)
        {
            if (RestService.LoggedUser == null)
            {
                Navigation.InsertPageBefore(new SignUp(), this);
            }
            else
            {
                Navigation.InsertPageBefore(new ProfilePage(RestService.LoggedUser), this);
            }
            await Navigation.PopAsync(false);
        }

        async void SwipedLeft(int index)
        {
            int idproduct = productCards.product.idproducts;
            if (RestService.LoggedUser != null)
            {
                string result = await App.Manager.RejectProduct(idproduct);
                if (result.Equals("FAIL"))
                {
                    notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al rechazar el producto", TimeSpan.FromSeconds(1));
                }
            }
            else
            {
                App.Database.SaveProduct(new Taste { idproducts = idproduct, inter_date = DateTime.Today, liked = false });
            }
            if (productCards.IsEnding)
            {
                List<Product> temp = await App.Manager.GetProducts();
                List<Product> actual = productCards.ItemsSource;
                productCards.ItemsSource.AddRange(temp.Where(x => !actual.Any(y => x.idproducts == y.idproducts)));
            }
        }

        async void SwipedRight(int index)
        {
            int idproduct = productCards.product.idproducts;
            if (RestService.LoggedUser != null)
            {
                string result = await App.Manager.LikeProduct(idproduct);
                if (result.Equals("FAIL"))
                {
                    notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al agregar el producto", TimeSpan.FromSeconds(1));
                }
            }
            else
            {
                App.Database.SaveProduct(new Taste { idproducts = idproduct, inter_date = DateTime.Today, liked = true });
            }
            if (productCards.IsEnding)
            {
                List<Product> temp = await App.Manager.GetProducts();
                List<Product> actual = productCards.ItemsSource;
                productCards.ItemsSource.AddRange(temp.Where(x => !actual.Any(y => x.idproducts == y.idproducts)));
            }
        }


    }
}
