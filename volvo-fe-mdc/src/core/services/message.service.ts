const messageService = {
  success: {
    MSG001: "O registro de {0} foi salvo com sucesso.",
    MSG002: "O registro de {0} foi importado com sucesso.",
    MSG003: "Os registros de {0} foram aplicados com sucesso.",
    MSG004: "Os registros de {0} foram excluídos com sucesso.",
    MSG005: "O registro de {0} foi atualizado com sucesso.",
  },
  error: {
    MSG001: "Erro ao salvar o registro de {0}.",
    MSG002: "Erro ao importar o registro de {0}.",
    MSG003: "Erro não identificado ao salvar o registro de {0}.",
    MSG004: "Erro ao aplicar o registro de {0}.",
    MSG005: "Erro ao excluir os registros de {0}.",
    MSG006: "Erro ao atualizar o registro de {0}.",
    MSG007: "Erro ao desfazer o registro de {0}.",
    MSG008: "Falha ao extrair relatório de {0}.",
    MSG009: "Falha ao extrair {0} de {1}.",
  },
  info: {
    MSG001: "Deseja aprovar todas as propostas de pagamento?",
    MSG002: "Você tem certeza que deseja cancelar todas as aprovações?",
    MSG003: "Esta ação irá executar Movimentações no caixa. Deseja realmente aplicar as transferências?",
    MSG004: "Esta ação permitirá atualizar de forma manual o saldo inicial de uma conta. Deseja realmente continuar?",
    MSG005: "Não é permitido atualizar o Saldo Inicial, pois já houve o Fechamento do Caixa",
    MSG006: "Não existem dados de {0} a serem salvos ",
    MSG007: "Esta ação permitirá desfazer a última conciliação salvar. Deseja realmente continuar?",
    MSG008: "Esta ação permitirá desfazer todas conciliações salvas. Deseja realmente continuar?",
    MSG009: "Registro já existente.",
    MSG010: "Deseja excluir o(s) documento(s) de pagamento selecionado(s)?",
    MSG011: "Os Documentos foram alterado, mas para aplicar salve o registro de {0}.",

  },
  
};

export default messageService;
