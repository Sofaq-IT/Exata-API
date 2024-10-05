using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Exata.Helpers;

public class PagedList<T> : List<T>
{
    public int PaginaAtual { get; private set; }

    public int TotalPaginas { get; private set; }

    public int RegistrosPorPagina { get; private set; }

    public int TotalRegistros { get; private set; }

    public bool TemAnterior => PaginaAtual > 1;

    public bool TemProximo => PaginaAtual < TotalPaginas;

    public PagedList(List<T> itens, int count, int pageNumeber, int pageSize)
    {
        TotalRegistros = count;
        RegistrosPorPagina = pageSize;
        PaginaAtual = pageNumeber;
        TotalPaginas = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(itens);
    }

    public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var itens = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(itens, count, pageNumber, pageSize);
    }

    public string JsonHeaderPaginacao()
    {
        var retPaginacao = new
        {
            TotalRegistros,
            RegistrosPorPagina,
            PaginaAtual,
            TotalPaginas,
            TemProximo,
            TemAnterior
        };

        return JsonSerializer.Serialize(retPaginacao);
    }
}