using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DiarioPessoal
{
    public partial class Form1 : Form
    {
        private string ficheiro = "";
        public Form1()
        {
            InitializeComponent();
        }
        private void GuardarFicheiro()
        {
            saveFileDialog1.Filter = "Ficheiros RTF | *.rtf | Ficheiros TXT | *.txt";
            saveFileDialog1.FileName = "";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ficheiro = saveFileDialog1.FileName;
                rbTexto.SaveFile(ficheiro);
                rbTexto.Modified = false;
            }
        }

        private void VerificarAlteracoes()
        {
            if (rbTexto.Modified == true)
            {
                DialogResult resposta = MessageBox.Show("Deseja guardar o texto atual?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (resposta == DialogResult.Yes)
                {
                    if (ficheiro != "")
                    {
                        rbTexto.SaveFile(ficheiro);
                        rbTexto.Modified = false;
                    }
                    else
                    {
                        GuardarFicheiro();
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void AtualizarContador()
        {
            // Conta caracteres
            int numCaracteres = rbTexto.Text.Length;

            // Conta palavras
            int numPalavras = rbTexto.Text.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

            // Atualiza a label na StatusBar
            toolStripStatusLabel1.Text = $"Caracteres: {numCaracteres} | Palavras: {numPalavras}";
        }

        private void EnableDarkMode()
        {
            // Habilita o modo escuro (tema escuro) em todos os controles do formulário
            foreach (Control control in Controls)
            {
                EnableDarkMode(control);
            }
        }

        private void EnableDarkMode(Control control)
        {
            // Habilita o modo escuro em um controle e em seus controles filhos recursivamente
            control.BackColor = System.Drawing.Color.FromArgb(169, 169, 169);
            control.ForeColor = System.Drawing.Color.White;

            // Se o controle for uma Label, define o texto em negrito
            if (control is Label)
            {
                (control as Label).Font = new System.Drawing.Font(control.Font, System.Drawing.FontStyle.Bold);
            }

            // Se o controle tiver controles filhos, aplique o modo escuro a eles também
            foreach (Control childControl in control.Controls)
            {
                EnableDarkMode(childControl);
            }
        }

        private void DisableDarkMode()
        {
            // Desabilita o modo escuro (retorna ao tema claro) em todos os controles do formulário
            foreach (Control control in Controls)
            {
                DisableDarkMode(control);
            }
        }

        private void DisableDarkMode(Control control)
        {
            // Desabilita o modo escuro em um controle e em seus controles filhos recursivamente
            control.BackColor = System.Drawing.SystemColors.Control;
            control.ForeColor = System.Drawing.SystemColors.ControlText;

            // Se o controle for uma Label, define o texto em negrito
            if (control is Label)
            {
                (control as Label).Font = new System.Drawing.Font(control.Font, System.Drawing.FontStyle.Regular);
            }

            // Se o controle tiver controles filhos, remova o modo escuro deles também
            foreach (Control childControl in control.Controls)
            {
                DisableDarkMode(childControl);
            }
        }

        private void menuFicheiroNovo_Click(object sender, EventArgs e)
        {
            VerificarAlteracoes();

            rbTexto.ResetText();
            rbTexto.Modified = false;
            ficheiro = null;
        }

        private void menuFicheiroAbrir_Click(object sender, EventArgs e)
        {
            VerificarAlteracoes();

            openFileDialog1.Filter = "Ficheiro RTF | *.rtf | Ficheiros TXT | *.txt | Todos | *.*";
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ficheiro = openFileDialog1.FileName;
                rbTexto.LoadFile(ficheiro);
                rbTexto.Modified = false;
            }
        }

        private void menuFicheiroGuardar_Click(object sender, EventArgs e)
        {
            if (ficheiro != "")
            {
                rbTexto.SaveFile(ficheiro);
                rbTexto.Modified = false;
            }
            else
            {
                GuardarFicheiro();
            }
        }

        private void menuFicheiroSair_Click(object sender, EventArgs e)
        {
            if (rbTexto.Text != "") // Verifica se há texto no editor
            {
                VerificarAlteracoes();
            }
            else
            {
                DialogResult resposta = MessageBox.Show("Deseja sair da aplicação?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resposta == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private void menuEditarCortar_Click(object sender, EventArgs e)
        {
            // Controlo para cortar o texto selecionado
            rbTexto.Cut();
        }

        private void menuEditarCopiar_Click(object sender, EventArgs e)
        {
            // Controlo para copiar o texto selecionado
            rbTexto.Copy();
        }

        private void menuEditarColar_Click(object sender, EventArgs e)
        {
            // Controlo para colar texto
            rbTexto.Paste();
        }

        private void menuEditarSelecionarTudo_Click(object sender, EventArgs e)
        {
            // Controlo para selecionar tudo
            rbTexto.SelectAll();
        }

        private void menuEditarProcurar_Click(object sender, EventArgs e)
        {
            // Controlo para procurar texto
            string txtProcura = Interaction.InputBox("Digite o que procura:", "Procurar", "", 150, 200);

            int resultado = rbTexto.Find(txtProcura);
            if (resultado == -1)
            {
                MessageBox.Show("Não foi encontrada a sua procura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void menuFormatarLetra_Click(object sender, EventArgs e)
        {
            if (rbTexto.SelectionFont != null)
            {
                fontDialog1.Font = rbTexto.SelectionFont;
            }
            else
            {
                fontDialog1.Font = null;
            }

            fontDialog1.ShowDialog();
            rbTexto.SelectionFont = fontDialog1.Font;
        }

        private void menuFormatarCoresLetras_Click(object sender, EventArgs e)
        {
            // Mostrar o controlador
            colorDialog1.ShowDialog();
            // Alterar a cor do texto selecionado para a cor escolhida no controlador
            rbTexto.SelectionColor = colorDialog1.Color;
        }

        private void menuFormatarCoresFundo_Click(object sender, EventArgs e)
        {
            // Mostrar o controlador
            colorDialog1.ShowDialog();
            // Alterar o fundo do texto selecionado para a cor escolhida no controlador
            rbTexto.SelectionBackColor = colorDialog1.Color;
        }

        private void menuFormatarAlinhamentoEsq_Click(object sender, EventArgs e)
        {
            rbTexto.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void menuFormatarAlinhamentoCen_Click(object sender, EventArgs e)
        {
            rbTexto.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void menuFormatarAlinhamentoDir_Click(object sender, EventArgs e)
        {
            rbTexto.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void rbTexto_TextChanged(object sender, EventArgs e)
        {
            AtualizarContador();
        }

        private void checkBoxTema_CheckedChanged(object sender, EventArgs e)
        {
            // Alterna entre os temas claro e escuro com base no estado da CheckBox
            if (checkBoxTema.Checked)
            {
                // Ativa o tema escuro
                EnableDarkMode();
            }
            else
            {
                // Desativa o tema escuro
                DisableDarkMode();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Obtém a data e hora atuais
            DateTime now = DateTime.Now;
            // Formata a data e hora no formato desejado
            string dataHoraFormatada = now.ToString("dd/MM/yyyy | HH:mm");
            // Exibe a data e hora no título do formulário ou em um controle de sua escolha
            lblData.Text = dataHoraFormatada;
        }
    }
}
