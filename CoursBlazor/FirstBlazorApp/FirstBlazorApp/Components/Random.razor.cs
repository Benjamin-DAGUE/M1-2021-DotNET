using Microsoft.AspNetCore.Components;

namespace FirstBlazorApp.Components;

//La classe doit être partielle pour compléter le composant associé.
public partial class Random
{
    private System.Random? _Random;
    
    [Parameter] //Parameter permet de définir que la propriété va pouvoir être liée par le composant parent.
    public int RandomNumber { get; set; }

    //Le paramètre RandomNumber prend en charge une liaison TwoWay seulement si
    //un paramètre nommé <ParameterName>Changed existe et est de type EventCallback<T>
    //avec T = type du paramètre.
    //Le callback doit être appelé par le composant lorsque la valeur change (cf. NewNumber()).
    [Parameter]
    public EventCallback<int> RandomNumberChanged { get; set; }

    protected override Task OnInitializedAsync() 
    {
        _Random = new System.Random();
        return Task.CompletedTask;
    }

    private void NewNumber()
    {
        //Cette version peut être simplifiée (cf ci-dessous)
        //RandomNumber = _Random?.Next(0, 100) ?? 0;
        ////Déclenche la mise à jour des liaison TwoWay pour le paramètre RandomNumber
        //RandomNumberChanged.InvokeAsync(RandomNumber);

        //Affecte le paramètre RandomNumber et met à jour les liaisons TwoWay
        RandomNumberChanged.InvokeAsync(_Random?.Next(0, 100) ?? 0);
    }
}
