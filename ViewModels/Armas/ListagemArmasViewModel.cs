using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;

namespace AppRpgEtec.ViewModels.Armas
{
    public class ListagemArmasViewModel : BaseViewModel
    {
        private ArmaService aService;

        public ObservableCollection<Arma> Armas { get; set; }

        public ListagemArmasViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmaService(token);
            Armas = new ObservableCollection<Arma>();

            _ = ObterArmas();

            NovaArmaCommand = new Command(async () => { await ExibirCadastroArma(); });
            RemoverArmaCommand = new Command<Arma>(async (Arma a) => { await RemoverArma(a); });
        }

        public ICommand NovaArmaCommand { get; }

        public ICommand RemoverArmaCommand { get; set; }
      
        public async Task ObterArmas()
        {
            try
            {
                Armas = await aService.GetArmasAsync();
                OnPropertyChanged(nameof(Armas));


            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + ex.InnerException, "Okay");
            }
        }

        public async Task ExibirCadastroArma()
        {
            try
            {
                await Shell.Current.GoToAsync("cadArmaView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "OK");

            }
        }

        private Arma armaSelecionada;
        public Arma ArmaSelecionada
        {
            get {   return armaSelecionada; }
            set
            {
                if (value != null)
                {
                    armaSelecionada = value; 
                  
                    Shell.Current.GoToAsync($"cadArmaView?pId={armaSelecionada.Id}");
                }
            }
        }
        private Personagem personagemSelecionado;
        public Personagem PersonagemSelecionado
        {
            get { return personagemSelecionado; }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;

                    Shell.Current.GoToAsync($"cadPersonagemView?pId={personagemSelecionado.Id}");
                }
            }
        }


        public async Task RemoverArma(Arma a)
        {
            try
            {
                if (await Application.Current.MainPage.
                    DisplayAlert("Confirmação", $"Confirma a remoção de {a.Nome}?", "Sim", "Não"))
                {
                    await aService.DeleteArmaAsync(a.Id);

                    await Application.Current.MainPage.
                        DisplayAlert("Mensagem", "Arma removida com sucesso!", "ok");
                }
            }
            catch (Exception ex)
            {
                    await Application.Current.MainPage.
                        DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}
