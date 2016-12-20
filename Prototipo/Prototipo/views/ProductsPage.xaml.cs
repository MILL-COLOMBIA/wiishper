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

            view.Children.Add(productCards,
                              Constraint.Constant(30),
                              Constraint.Constant(60),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width - 60;
                              }),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Height - 240;
                              })
                );

            this.LayoutChanged += (object sender, System.EventArgs e) =>
            {
                productCards.CardMoveDistance = (int)(this.Width * 0.60f);
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

            StackLayout bottomMenu = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.White
            };

            Button friends = new Button
            {
                Image = "people.png",
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BorderRadius = 0
            };

            friends.Clicked += OnFriends;

            Button product_btn = new Button
            {
                Image = "main.png",
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BorderRadius = 0
            };

            product_btn.Clicked += OnProducts;

            Button profile = new Button
            {
                Image = "profile.png",
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BorderRadius = 0
            };

            profile.Clicked += OnProfile;

            bottomMenu.Children.Add(friends);
            bottomMenu.Children.Add(product_btn);
            bottomMenu.Children.Add(profile);

            view.Children.Add(bottomMenu,
                              Constraint.Constant(0),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Height - 43;
                              }),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width;
                              }),
                              Constraint.Constant(43));

            BoxView bottomSeparator = new BoxView
            {
                BackgroundColor = Color.FromHex("80BCBEC0")
            };

            view.Children.Add(bottomSeparator,
                              Constraint.Constant(0),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Height - 45;
                              }
                              ),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width;
                              }),
                              Constraint.Constant(2));

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
                                  return parent.Width - 123;
                              }),
                              Constraint.RelativeToView(productCards,
                              (parent, sibling) =>
                              {
                                  return sibling.Y + sibling.Height + 60;
                              }),
                              Constraint.Constant(73),
                              Constraint.Constant(73));

            view.Children.Add(dislike,
                              Constraint.Constant(50),
                              Constraint.RelativeToView(productCards,
                              (parent, sibling) =>
                              {
                                  return sibling.Y + sibling.Height + 60;
                              }),
                              Constraint.Constant(73),
                              Constraint.Constant(73));

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
                if (result.Equals("FAIL"))
                {
                    notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al rechazar el producto", TimeSpan.FromSeconds(2));
                }
                else
                {
                    await productCards.NextLeft();
                    notificator.Notify(ToastNotificationType.Error, "Wiishper", "Producto rechazado", TimeSpan.FromSeconds(2));
                }
            }
            else
            {
                App.Database.SaveProduct(new Taste { idproducts = idproduct, inter_date = new DateTime(), liked = false });
                await productCards.NextLeft();
                notificator.Notify(ToastNotificationType.Error, "Wiishper", "Producto rechazado", TimeSpan.FromSeconds(2));
            }
            if (productCards.IsEnding)
                productCards.ItemsSource.AddRange(await App.Manager.GetProducts());
        }

        private async void OnLike(object sender, EventArgs e)
        {
            int idproduct = productCards.product.idproducts;
            if (RestService.LoggedUser != null)
            {
                string result = await App.Manager.LikeProduct(idproduct);
                if (result.Equals("FAIL"))
                {
                    notificator.Notify(ToastNotificationType.Error, "Wiishper", "Error al agregar el producto", TimeSpan.FromSeconds(2));
                }
                else
                {
                    await productCards.NextRight();
                    notificator.Notify(ToastNotificationType.Success, "Wiishper", "Producto agregado", TimeSpan.FromSeconds(2));
                }
            }
            else
            {
                App.Database.SaveProduct(new Taste { idproducts = idproduct, inter_date = new DateTime(), liked = true });
                await productCards.NextRight();
                notificator.Notify(ToastNotificationType.Success, "Wiishper", "Producto agregado", TimeSpan.FromSeconds(2));
            }
            if (productCards.IsEnding)
                productCards.ItemsSource.AddRange(await App.Manager.GetProducts());
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
            await Navigation.PopAsync();
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
            await Navigation.PopAsync();
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
            await Navigation.PopAsync();
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
            await Navigation.PopAsync();
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
                else
                {
                    notificator.Notify(ToastNotificationType.Error, "Wiishper", "Producto rechazado", TimeSpan.FromSeconds(1));
                }
            }
            else
            {
                App.Database.SaveProduct(new Taste { idproducts = idproduct, inter_date = DateTime.Today, liked = false });
                notificator.Notify(ToastNotificationType.Error, "Wiishper", "Producto rechazado", TimeSpan.FromSeconds(1));
            }
            if (productCards.IsEnding)
                productCards.ItemsSource.AddRange(await App.Manager.GetProducts());
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
                else
                {
                    notificator.Notify(ToastNotificationType.Success, "Wiishper", "Producto agregado", TimeSpan.FromSeconds(1));
                }
            }
            else
            {
                App.Database.SaveProduct(new Taste { idproducts = idproduct, inter_date = DateTime.Today, liked = true });
                notificator.Notify(ToastNotificationType.Success, "Wiishper", "Producto agregado", TimeSpan.FromSeconds(1));
            }
            if (productCards.IsEnding)
                productCards.ItemsSource.AddRange(await App.Manager.GetProducts());
        }


    }
}
