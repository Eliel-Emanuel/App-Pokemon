using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Store;

namespace LeerDB
{
    public partial class frmPokedex : Form
    {
        private List<Pokemon> listaPokemon;
        public frmPokedex()
        {
            InitializeComponent();
        }

        private void cargar()
        {
            try
            {
                PokemonNegocio store = new PokemonNegocio();
                listaPokemon = store.listar();
                dgvPokemon.DataSource = listaPokemon;
                ocultarColumnas();
                pbxPokemon.Load(listaPokemon[0].Imagen);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ocultarColumnas()
        {
            dgvPokemon.Columns["Imagen"].Visible = false;
            dgvPokemon.Columns["Id"].Visible = false;
        }

        private void frmPokedex_Load(object sender, EventArgs e)
        {
           cargar();

            cboxCampo.Items.Add("Número");
            cboxCampo.Items.Add("Nombre");
            cboxCampo.Items.Add("Descripción");
        }

        private void dgvPokemon_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvPokemon.CurrentRow != null)
            {
                Pokemon selecionado = (Pokemon)dgvPokemon.CurrentRow.DataBoundItem;
                CargarImagen(selecionado.Imagen);
            }
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception)
            {
                pbxPokemon.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/681px-Placeholder_view_vector.svg.png");
                
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            frmAltaPokemon alta = new frmAltaPokemon();
            alta.ShowDialog();
            cargar();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemon.CurrentRow.DataBoundItem;

            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                PokemonNegocio conexion = new PokemonNegocio();
                Pokemon seleccionado;
                seleccionado = (Pokemon)dgvPokemon.CurrentRow.DataBoundItem;

                conexion.delete(seleccionado.Id);
                MessageBox.Show("Pokemon deleted");
                cargar();
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void tbxFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Pokemon> listaFiltrada;

            if (tbxFiltro.Text != "")
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToLower().Contains(tbxFiltro.Text.ToLower()));
            else
                listaFiltrada = listaPokemon;

            dgvPokemon.DataSource = null;
            dgvPokemon.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboxCampo.SelectedItem.ToString();

            if(opcion == "Número"){

                cboxCriterio.Items.Clear();
                cboxCriterio.Items.Add("Mayor a");
                cboxCriterio.Items.Add("Menor a");
                cboxCriterio.Items.Add("Igual a");

            }
            else
            {
                cboxCriterio.Items.Clear();
                cboxCriterio.Items.Add("Comienza con");
                cboxCriterio.Items.Add("Termina con");
                cboxCriterio.Items.Add("Contiene");
            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                string campo = cboxCampo.SelectedItem.ToString();
                string criterio = cboxCriterio.SelectedItem.ToString();
                string filtro = tbxFiltroAvanzado.Text;
                dgvPokemon.DataSource = negocio.Filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
    
}