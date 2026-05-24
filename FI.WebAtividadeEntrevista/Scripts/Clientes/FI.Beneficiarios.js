/**
 * Gerenciador de Beneficiários
 */
var BeneficiariosManager = {
    beneficiarios: [], // Array para armazenar beneficiários
    editandoIndex: -1, // Índice do beneficiário sendo editado

    /**
     * Inicializa o gerenciador
     */
    init: function () {
        this.bindEvents();
        this.renderGrid();
    },

    /**
     * Vincula eventos aos botões
     */
    bindEvents: function () {
        var self = this;

        // Abrir modal
        $('#btnBeneficiarios').click(function (e) {
            e.preventDefault();
            $('#modalBeneficiarios').modal('show');
        });

        // Incluir/Alterar beneficiário
        $('#btnIncluirBenef').click(function () {
            self.salvarBeneficiario();
        });

        // Cancelar edição
        $('#btnCancelarEdicao').click(function () {
            self.cancelarEdicao();
        });

        // Aplicar máscara de CPF
        CpfValidator.aplicarMascara('#benef-cpf');
    },

    /**
     * Salva (inclui ou altera) um beneficiário
     */
    salvarBeneficiario: function () {
        var cpf = $('#benef-cpf').val().trim();
        var nome = $('#benef-nome').val().trim();

        // Validações
        if (!cpf) {
            ModalDialog('Atenção', 'CPF do beneficiário é obrigatório');
            return;
        }

        if (!CpfValidator.isValid(cpf)) {
            ModalDialog('Atenção', 'CPF do beneficiário inválido');
            return;
        }

        if (!nome) {
            ModalDialog('Atenção', 'Nome do beneficiário é obrigatório');
            return;
        }

        // Verifica CPF duplicado
        var cpfLimpo = CpfValidator.RemoverFormatacao(cpf);
        var isDuplicado = this.beneficiarios.some(function (b, index) {
            return CpfValidator.RemoverFormatacao(b.CPF) === cpfLimpo &&
                index !== BeneficiariosManager.editandoIndex;
        });

        if (isDuplicado) {
            ModalDialog('Atenção', 'Já existe um beneficiário com este CPF');
            return;
        }

        // Incluir ou alterar
        if (this.editandoIndex >= 0) {
            // Alterar
            this.beneficiarios[this.editandoIndex].CPF = cpf;
            this.beneficiarios[this.editandoIndex].Nome = nome;
            this.editandoIndex = -1;
            $('#btnCancelarEdicao').hide();
            $('#btnIncluirBenef').text('Incluir').removeClass('btn-warning').addClass('btn-success');
        } else {
            // Incluir
            this.beneficiarios.push({
                CPF: cpf,
                Nome: nome
            });
        }

        // Limpar campos e atualizar grid
        this.limparCampos();
        this.renderGrid();
    },

    /**
     * Editar um beneficiário
     */
    editarBeneficiario: function (index) {
        var benef = this.beneficiarios[index];

        $('#benef-cpf').val(benef.CPF);
        $('#benef-nome').val(benef.Nome);

        this.editandoIndex = index;
        $('#btnIncluirBenef').text('Salvar Alteração').removeClass('btn-success').addClass('btn-warning');
        $('#btnCancelarEdicao').show();

        // Scroll para o topo do modal
        $('.modal-body').animate({ scrollTop: 0 }, 300);
    },

    /**
     * Excluir um beneficiário
     */
    excluirBeneficiario: function (index) {
        if (confirm('Deseja realmente excluir este beneficiário?')) {
            this.beneficiarios.splice(index, 1);
            this.renderGrid();
        }
    },

    /**
     * Cancelar edição
     */
    cancelarEdicao: function () {
        this.editandoIndex = -1;
        this.limparCampos();
        $('#btnIncluirBenef').text('Incluir').removeClass('btn-warning').addClass('btn-success');
        $('#btnCancelarEdicao').hide();
    },

    /**
     * Limpa os campos do formulário
     */
    limparCampos: function () {
        $('#benef-cpf').val('');
        $('#benef-nome').val('');
    },

    /**
     * Renderiza o grid de beneficiários
     */
    renderGrid: function () {
        var tbody = $('#gridBeneficiarios tbody');
        tbody.empty();

        if (this.beneficiarios.length === 0) {
            $('#gridBeneficiarios').hide();
            $('#msgSemBeneficiarios').show();
            return;
        }

        $('#gridBeneficiarios').show();
        $('#msgSemBeneficiarios').hide();

        this.beneficiarios.forEach(function (benef, index) {
            var tr = $('<tr>');
            tr.append('<td>' + benef.CPF + '</td>');
            tr.append('<td>' + benef.Nome + '</td>');
            tr.append(
                '<td>' +
                '<button type="button" class="btn btn-primary btn-sm" onclick="BeneficiariosManager.editarBeneficiario(' + index + ')">Alterar</button> ' +
                '<button type="button" class="btn btn-danger btn-sm" onclick="BeneficiariosManager.excluirBeneficiario(' + index + ')">Excluir</button>' +
                '</td>'
            );
            tbody.append(tr);
        });
    },

    /**
     * Carrega beneficiários existentes (modo edição)
     */
    carregarBeneficiarios: function (idCliente) {
        var self = this;

        $.ajax({
            url: '/Cliente/ListarBeneficiarios',
            method: 'GET',
            data: { idCliente: idCliente },
            success: function (data) {
                self.beneficiarios = data;
                self.renderGrid();
            },
            error: function () {
                console.error('Erro ao carregar beneficiários');
            }
        });
    },

    /**
     * Retorna os beneficiários para enviar ao servidor
     */
    obterBeneficiarios: function () {
        return this.beneficiarios;
    }
};

// Inicializar quando o documento estiver pronto
$(document).ready(function () {
    BeneficiariosManager.init();
});