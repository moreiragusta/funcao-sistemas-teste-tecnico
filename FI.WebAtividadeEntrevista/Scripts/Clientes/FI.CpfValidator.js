/**
 * Utilitários para validação de CPF
 */
var CpfValidator = {
    /**
     * Valida um CPF brasileiro
     * @param {string} cpf - CPF com ou sem formatação
     * @returns {boolean} - True se válido, False caso contrário
     */
    isValid: function (cpf) {
        // Remove formatação
        cpf = cpf.replace(/[^\d]/g, '');

        // Verifica se tem 11 dígitos
        if (cpf.length !== 11) return false;

        // Verifica se todos os dígitos são iguais
        if (/^(\d)\1{10}$/.test(cpf)) return false;

        // Valida primeiro dígito verificador
        var soma = 0;
        for (var i = 0; i < 9; i++) {
            soma += parseInt(cpf.charAt(i)) * (10 - i);
        }
        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        if (parseInt(cpf.charAt(9)) !== digito1) return false;

        // Valida segundo dígito verificador
        soma = 0;
        for (var i = 0; i < 10; i++) {
            soma += parseInt(cpf.charAt(i)) * (11 - i);
        }
        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return parseInt(cpf.charAt(10)) === digito2;
    },

    /**
     * Remove formatação do CPF
     * @param {string} cpf - CPF formatado
     * @returns {string} - CPF apenas com números
     */
    removerFormatacao: function (cpf) {
        return cpf.replace(/[^\d]/g, '');
    },

    /**
     * Formata CPF com pontos e hífen
     * @param {string} cpf - CPF sem formatação
     * @returns {string} - CPF formatado (999.999.999-99)
     */
    formatar: function (cpf) {
        cpf = cpf.replace(/[^\d]/g, '');
        return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
    },

    /**
     * Aplica máscara de CPF em um input
     * @param {string} selector - Seletor CSS do input
     */
    aplicarMascara: function (selector) {
        $(selector).on('input', function () {
            var value = $(this).val().replace(/\D/g, '');

            if (value.length <= 11) {
                value = value.replace(/(\d{3})(\d)/, '$1.$2');
                value = value.replace(/(\d{3})(\d)/, '$1.$2');
                value = value.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
            }

            $(this).val(value);
        });
    }
};