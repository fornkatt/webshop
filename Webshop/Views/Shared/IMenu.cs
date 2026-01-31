namespace Webshop.Views.Shared;

internal interface IMenu
{
    Task ActivateAsync();
    string HeaderText { get; set; }
    Task RenderMenuAsync();
}