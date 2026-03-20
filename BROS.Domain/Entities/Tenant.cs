using BROS.Domain.Abstractions;

namespace BROS.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Nome { get; private set; }
    public string Subdominio { get; private set; }
    public bool Ativo { get; private set; }

    public Tenant(string nome, string subdominio)
    {
        Validar(nome, subdominio);

        Nome = nome;
        Subdominio = subdominio;
        Ativo = true;
    }

    private static void Validar (string nome, string subdominio)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do lojista é obrigatório.");

        if (string.IsNullOrWhiteSpace(subdominio)) 
            throw new ArgumentException("O subdomínio é obrigatório para o isolamento do lojista");
    }
}
