using Backend.DB;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Controllers {
    [Route("api/v1")]
    public class BackendController : Controller {
        private readonly ILogger<BackendController> _logger;
        private readonly ConnectionMySQL _connection;

        private string _secretKey = "segredoparatodosostiposdetrouxasXD";


        public BackendController(ConnectionMySQL connection, ILogger<BackendController> logger) {
            _connection = connection;
            _logger = logger;
        }

        #region  Usuários
        [HttpGet("getUsers")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers() {
            return await _connection.Users.ToListAsync();
        }

        [HttpGet("getUser/{id}")]
        public async Task<ActionResult> GetUser(int id) {
            var user = await _connection.Users.FindAsync(id);

            if (user == null) {
                return BadRequest("Usuário não encontrado");
            }

            return Ok(user);
        }

        [HttpPost("postUser")]
        public async Task<ActionResult<UserViewModel>> PostUser(UserViewModel user) {
            _connection.Users.Add(user);
            await _connection.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<ActionResult<UserViewModel>> DelUser(int id) {
            var user = await _connection.Users.FindAsync(id);

            if (user == null) {
                return NotFound();
            }

            _connection.Users.Remove(user);
            await _connection.SaveChangesAsync();

            return user;
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserViewModel user) {
            var FindUser = await _connection.Users.FindAsync(id);

            if (FindUser == null) {
                return NotFound("Usuário não encontrado");
            }

            string username = user.UserName;
            string email = user.Email;
            string password = user.Password;

            if (username == null) username = FindUser.UserName;
            if (email == null) email = FindUser.Email;
            if (password == null) password = FindUser.Password;

            FindUser.UserName = username;
            FindUser.Email = email;
            FindUser.Password = password;

            _connection.Users.Entry(FindUser).State = EntityState.Modified;

            try {
                await _connection.SaveChangesAsync();
                return Ok(FindUser);
            }
            catch (DbUpdateConcurrencyException err) {
                return BadRequest(err);
            }

        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> Login(UserViewModel model) {
            var user = await _connection.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == model.Password); // buscar usuário pelas credencias fornecidas

            if (user == null) {
                return Unauthorized(); // Credenciais inválidas
            }

            // Autenticado com sucesso, gerar e retornar token de autenticação

            string id = $"{model.Id_User}"; // id do usuario

            var token = GenerateToken(id, model.UserName, _secretKey); // gerar token com base nas informações
            return Ok(new { Token = token, Usuario = user }); // retornar o token e o usuário
        }
        #endregion
        #region Clientes 
        [HttpGet("GetClients")]
        public async Task<ActionResult<IEnumerable<ClientesViewModel>>> GetClients() {
            return await _connection.Clientes.ToListAsync();
        }

        [HttpGet("GetClient/{id}")]
        public async Task<ActionResult<ClientesViewModel>> GetClient(string id) {
            var client = await _connection.Clientes.FindAsync(id);

            if(client == null) {
                return NotFound("Cliente não encontrado");
            }

            return Ok(client);
        }

        [HttpPost("PostClient")]
        public async Task<ActionResult<ClientesViewModel>> PostClient(ClientesViewModel client) {
            _connection.Clientes.Add(client);
            await _connection.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("DeleteClient/{id}")]
        public async Task<ActionResult<ClientesViewModel>> DeleteClient(int id) {
            var client = await _connection.Clientes.FindAsync(id);

            if(client == null) {
                return NotFound("Cliente não encontrado");
            }

            _connection.Clientes.Remove(client);
            await _connection.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("PutClient/{id}")]
        public async Task<ActionResult<ClientesViewModel>> PutClient(int id,ClientesViewModel client) {
            var findClient = await _connection.Clientes.FindAsync(id);

            if(findClient == null) {
                return NotFound("Cliente não encontrado");
            }

            string cliente = client.Cliente;
            string email = client.Email;
            string telefone = client.Telefone;
            string endereco = client.Endereco;
            DateOnly date = client.DataDeNascimento;
            char sexo = client.Sexo;

            if (cliente == "") cliente = findClient.Cliente;
            if (email == "") email = findClient.Email;
            if (telefone == "") telefone = findClient.Telefone;
            if (endereco == "") endereco = findClient.Endereco;
            if (date == null) date = findClient.DataDeNascimento;
            if (sexo.ToString() == "") sexo = findClient.Sexo;

            findClient.Cliente = cliente;
            findClient.Email = email;
            findClient.Telefone = telefone;
            findClient.Endereco = endereco;
            findClient.DataDeNascimento = date;
            findClient.Sexo = sexo;

            _connection.Clientes.Entry(findClient).State = EntityState.Modified;

            try {
                await _connection.SaveChangesAsync();
                return Ok(client);
            }catch(DbUpdateConcurrencyException db) {
                return BadRequest(db);
            }

        }
        #endregion
        #region Produtos

        [HttpPost("postProd")]
        public async Task<ActionResult<ProductsViewModel>> PostProduct(ProductsViewModel product) {

            if (string.IsNullOrWhiteSpace(product.Descricao)) {
                return BadRequest("A Descrição é obrigatória");
            }

            _connection.Products.Add(product);
            await _connection.SaveChangesAsync();
            return Ok(product);
        }

        [HttpGet("getProducts")]
        public async Task<ActionResult<IEnumerable<ProductsViewModel>>> GetProducts() {
            return await _connection.Products.ToListAsync();
        }

        [HttpGet("getProduct/{id}")]
        public async Task<ActionResult> GetProduct(int id) {

            var produto = await _connection.Products.FirstOrDefaultAsync(p => p.Id_Product == id);

            if (produto == null) {
                return BadRequest("Usuário não encontrado");
            }

            return Ok(produto);
        }

        [HttpPut("putProduct/{id}")]
        public async Task<ActionResult> PutProduct(int id, ProductsViewModel product) {

            var FindProduct = await _connection.Products.FindAsync(id);

            if (FindProduct == null) {
                return NotFound("Nenhum Produto foi encontrado");
            }

            string produto = product.Produto;
            string descricao = product.Descricao;
            decimal valor = product.Valor;


            if (produto == null) produto = FindProduct.Produto;
            if (descricao == null) descricao = FindProduct.Descricao;
            if (valor == 0) valor = FindProduct.Valor;


            FindProduct.Produto = produto;
            FindProduct.Descricao = descricao;
            FindProduct.Valor = valor;

            _connection.Products.Entry(FindProduct).State = EntityState.Modified;

            try {
                await _connection.SaveChangesAsync();
                return Ok(FindProduct);
            }
            catch (DbUpdateConcurrencyException erro) {
                return BadRequest(erro);
            }

        }

        [HttpDelete("deleteProduct/{id}")]
        public async Task<ActionResult<ProductsViewModel>> DeleteProduct(int id) {

            var user = await _connection.Products.FindAsync(id);

            if (user == null) {
                return BadRequest();
            }

            _connection.Products.Remove(user);
            await _connection.SaveChangesAsync();

            return user;
        }
        #endregion
        #region  Vendas
        [HttpPost("postVendas")]
        public async Task<ActionResult<VendasViewModel>> PostVendas(VendasViewModel vendas, UserViewModel user, ProductsViewModel product) {
            var findUser = await _connection.Users.FindAsync(user.Id_User);
            var findProduct = await _connection.Products.FindAsync(product.Id_Product);

            if (findUser == null) {
                return NotFound($"Esse Usuário não existe, id encontrado: {findUser}");
            }

            if (findProduct == null) {
                return NotFound("Esse Produto não existe");
            }

            vendas.Id_User = findUser.Id_User;
            vendas.Id_Product = findProduct.Id_Product;

            var total = vendas.Quantidade * vendas.Preco_Unidade;
            vendas.Valor_total = total;

            _connection.Vendas.Add(vendas);
            await _connection.SaveChangesAsync();

            return Ok(vendas);
        }

        [HttpGet("getAllVendas")]
        public async Task<ActionResult<IEnumerable<VendasViewModel>>> GetAllVendas() {

            var lista = await _connection.Vendas.ToListAsync();

            return Ok(lista);
        }

        [HttpGet("getVendas/{id}")]
        public async Task<ActionResult<VendasViewModel>> GetVendas(int id) {

            var venda = await _connection.Vendas.FindAsync(id);

            if (venda == null) { return NotFound("Venda não encontrada"); }
            return Ok(venda);
        }

        [HttpPut("putVendas/{id}")]
        public async Task<ActionResult> PutVendas(int id, VendasViewModel vendas) {

            var findVenda = await _connection.Vendas.FindAsync(id);

            if (findVenda == null) {
                return NotFound("Venda não foi encontrada!");
            }

            int id_user = vendas.Id_User;
            int id_product = vendas.Id_Product;
            int quantidade = vendas.Quantidade;
            decimal preco_unidade = vendas.Preco_Unidade;
            if (id_user == 0) {
                id_user = findVenda.Id_User;
            }
            if (id_product == 0) {
                id_product = findVenda.Id_Product;
            }
            if (quantidade == 0) {
                quantidade = findVenda.Quantidade;
            }
            if (preco_unidade == 0) {
                preco_unidade = findVenda.Preco_Unidade;
            }




            findVenda.Id_User = id_user;
            findVenda.Id_Product = id_product;
            findVenda.Quantidade = quantidade;
            findVenda.Preco_Unidade = preco_unidade;
            decimal soma = quantidade * preco_unidade;
            findVenda.Valor_total = soma;


            _connection.Vendas.Entry(findVenda).State = EntityState.Modified;

            try {
                await _connection.SaveChangesAsync();
                return Ok(findVenda);
            }
            catch (DbUpdateConcurrencyException erro) {
                return BadRequest($"Ocorreu um erro e não foi possível alterar os dados.\n Erro encontrado: {erro}");
            }
        }

        [HttpDelete("deleteVendas/{id}")]
        public async Task<ActionResult<VendasViewModel>> DeleteVendas(int id) {

            var vendas = await _connection.Vendas.FindAsync(id);

            if (vendas == null) {
                return NotFound("Não encontrado");
            }

            _connection.Vendas.Remove(vendas);
            await _connection.SaveChangesAsync();
            return Ok();
        }
        #endregion
        #region  Vendas_Item
        [HttpPost("postVendas/item")]
        public async Task<ActionResult<VendasItemViewModel>> PostVendasItem(VendasItemViewModel vendas, VendasViewModel venda, ProductsViewModel products) {

            var FindVendas = await _connection.Vendas.FindAsync(vendas.Id_Vendas);
            if (FindVendas == null) return NotFound("Essa venda não existe");

            var FindProduct = await _connection.Products.FindAsync(vendas.Id_product);
            if (FindProduct == null) return NotFound("Esse produto não existe");

            var soma = vendas.Preco_Unidade * vendas.Quantidade;
            vendas.Valor_Total = soma;
            _connection.Vendas_Item.Add(vendas);
            await _connection.SaveChangesAsync();
            return Ok(vendas);
        }

        [HttpGet("getAllVendas/item")]
        public async Task<ActionResult<IEnumerable<VendasItemViewModel>>> GetAllVendasItem() {
            var vendasItem = await _connection.Vendas_Item.ToListAsync();
            return Ok(vendasItem);
        }

        [HttpGet("getVenda/item/{id}")]
        public async Task<ActionResult<VendasItemViewModel>> GetVendaItem(int id) {
            var vendaItem = await _connection.Vendas_Item.FindAsync(id);

            if (vendaItem == null) return NotFound("Venda especifica não encontrada");
            return Ok(vendaItem);
        }

        [HttpPut("putVenda/item/{id}")]
        public async Task<ActionResult<VendasItemViewModel>> PutVendaItem(int id, VendasItemViewModel vendaItem) {
            var venda = await _connection.Vendas_Item.FindAsync(id);
            if (venda == null) return NotFound("Item da venda não encontrado");

            int id_vendas = vendaItem.Id_Vendas;
            if(id_vendas == 0) id_vendas = venda.Id_Vendas;

            int id_produto = vendaItem.Id_product;
            if (id_produto == 0) id_produto = venda.Id_product;

            int quantidade = vendaItem.Quantidade;
            if (quantidade == 0) quantidade = venda.Quantidade;

            decimal preco_unidade = vendaItem.Preco_Unidade;
            if (preco_unidade == 0) preco_unidade = venda.Preco_Unidade;

            decimal soma = preco_unidade * quantidade;
            venda.Id_Vendas = id_vendas;
            venda.Id_product = id_produto;
            venda.Quantidade = quantidade;
            venda.Preco_Unidade = preco_unidade;
            venda.Valor_Total = soma;

            _connection.Vendas_Item.Entry(venda).State = EntityState.Modified;

            try {
                await _connection.SaveChangesAsync();
                return Ok(venda);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpDelete("deleteVenda/item/{id}")]
        public async Task<ActionResult> DeleteVendaItem(int id) {
            var findVenda = await _connection.Vendas_Item.FindAsync(id);
            if (findVenda == null) return NotFound("Item não encontrado");

            _connection.Vendas_Item.Remove(findVenda);
            await _connection.SaveChangesAsync();
            return Ok("Item deletado");
        }
        #endregion
        #region   Categoria_Produtos
        [HttpPost("postCategoria/produtos")]
        public async Task<ActionResult<CategoriaProdutosViewModel>> PostCategoriaProdutos(CategoriaProdutosViewModel categoria) {
            _connection.Categoria_Produtos.Add(categoria);
            await _connection.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpGet("getAllCategorias/produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaProdutosViewModel>>> GetAllCategoriasProdutos() {
            var categoria = await _connection.Categoria_Produtos.ToListAsync();
            return Ok(categoria);
        }

        [HttpGet("getCategoria/produto/{id}")]
        public async Task<ActionResult<CategoriaProdutosViewModel>> GetCategoriaProduto(int id) {
            var categoria = await _connection.Categoria_Produtos.FindAsync(id);
            if (categoria == null) return NotFound("Venda especifica não encontrada");
            return Ok(categoria);
        }

        [HttpPut("putCategoria/produto{id}")]
        public async Task<ActionResult<CategoriaProdutosViewModel>> PutVendaItem(int id, CategoriaProdutosViewModel categoria) {
            var findCategoria = await _connection.Categoria_Produtos.FindAsync(id);
            if (findCategoria == null) return NotFound("Item da venda não encontrado");

            string nomeCategoria = categoria.Nome_Categoria;
            if (nomeCategoria == null) nomeCategoria = findCategoria.Nome_Categoria;

            string descricao = categoria.Descricao;
            if (descricao == null) descricao = findCategoria.Descricao;
           
            findCategoria.Nome_Categoria = nomeCategoria;
            findCategoria.Descricao = descricao;
      
            _connection.Categoria_Produtos.Entry(findCategoria).State = EntityState.Modified;

            try {
                await _connection.SaveChangesAsync();
                return Ok(findCategoria);
            }
            catch (Exception) {
                return BadRequest();
            }
        }

        [HttpDelete("deleteCategoria/produto/{id}")]
        public async Task<ActionResult> DeleteCategoria(int id) {
            var findVenda = await _connection.Vendas_Item.FindAsync(id);
            if (findVenda == null) return NotFound("Item não encontrado");

            _connection.Vendas_Item.Remove(findVenda);
            await _connection.SaveChangesAsync();
            return Ok("Item deletado");
        }
        #endregion

        public string GenerateToken(string userId, string username, string secret) {
            var tokenHandler = new JwtSecurityTokenHandler(); // criar variável que recebe a função de criar tokens
            var key = Encoding.ASCII.GetBytes(secret); // transformar a secret key em bytes
            var tokenDescriptor = new SecurityTokenDescriptor { // descrever como será o token
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Defina o tempo de expiração do token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor); //criar o token
            return tokenHandler.WriteToken(token); // retonar o token criado
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
