using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Dominio;
using Store;

namespace LeerDB
{
    public partial class frmAltaPokemon : Form
    {
        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;

        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        public frmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text = "Modify";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAcept_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if(pokemon == null)
                    pokemon = new Pokemon();

                pokemon.Numero = int.Parse(tbxNumero.Text);
                pokemon.Nombre = tbxNombre.Text;
                pokemon.Descripcion = tbxDescripcion.Text;
                pokemon.Imagen = tbxUrlImagen.Text;
                pokemon.Tipo = (Elemento)cbxTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cbxDebilidad.SelectedItem;

                if (pokemon.Id != 0)
                {
                    negocio.modify(pokemon);
                    MessageBox.Show("Pokemon Modified");
                }
                else
                {
                    negocio.add(pokemon);
                    MessageBox.Show("Pokemon aggregated");
                }

            }
            catch ( Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

            if (archivo != null && !(tbxUrlImagen.Text.ToUpper().Contains("HTTP")))
            {
                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["imgFolder"] + archivo.SafeFileName);
            }

            Close();
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio negocio = new ElementoNegocio();
            try
            {
                cbxDebilidad.DataSource = negocio.listar();
                cbxDebilidad.ValueMember = "Id";
                cbxDebilidad.DisplayMember = "Descripcion";
                cbxTipo.DataSource = negocio.listar();
                cbxTipo.ValueMember = "Id";
                cbxTipo.DisplayMember = "Descripcion";


                if (pokemon != null)
                {
                    tbxNombre.Text = pokemon.Nombre;
                    tbxNumero.Text = pokemon.Numero.ToString();
                    tbxDescripcion.Text = pokemon.Descripcion;
                    tbxUrlImagen.Text = pokemon.Imagen;
                    CargarImagen(pokemon.Imagen);
                    cbxTipo.SelectedValue = pokemon.Tipo.Id;
                    cbxDebilidad.SelectedValue = pokemon.Debilidad.Id;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void tbxUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(tbxUrlImagen.Text);
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxUrlImagen.Load(imagen);
            }
            catch (Exception)
            {
                pbxUrlImagen.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/681px-Placeholder_view_vector.svg.png");

            }
        }

        private void btnImagenLocal_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter= "jpg|*.jpg|png|*.png";

            if(archivo.ShowDialog() == DialogResult.OK)
            {
                tbxUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);

                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["imgFolder"] + archivo.SafeFileName);
            }
        }

    }
}
