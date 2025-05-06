using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevXpertHub.Core.Entities;
using DevXpertHub.Infrastructure;

namespace DevXpertHub.Web.Views.Produtos
{
    public class ProdutosDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ProdutosDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProdutosDetails
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Produtos.Include(p => p.Categoria).Include(p => p.Fornecedor);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ProdutosDetails/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // GET: ProdutosDetails/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id");
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id");
            return View();
        }

        // POST: ProdutosDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Preco,Estoque,CategoriaId,FornecedorId,ImagemPrincipal,Id")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", produto.CategoriaId);
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id", produto.FornecedorId);
            return View(produto);
        }

        // GET: ProdutosDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", produto.CategoriaId);
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id", produto.FornecedorId);
            return View(produto);
        }

        // POST: ProdutosDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Nome,Descricao,Preco,Estoque,CategoriaId,FornecedorId,ImagemPrincipal,Id")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Id", produto.CategoriaId);
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "Id", "Id", produto.FornecedorId);
            return View(produto);
        }

        // GET: ProdutosDetails/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // POST: ProdutosDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(string id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
