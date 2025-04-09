using AppRpgEtec.ViewModels.Armas;

namespace AppRpgEtec.Views.Armas;

public partial class CadastroArmaView : ContentPage
{
	private CadastroArmaViewModel cadArmaViewModel;
	public CadastroArmaView()
	{
		InitializeComponent();
		
		cadArmaViewModel = new CadastroArmaViewModel();
		BindingContext = cadArmaViewModel;
		Title = "Nova Arma";
	
	}
}