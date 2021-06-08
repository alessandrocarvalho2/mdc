const LayoutService: any = {
  /**
   * Retorna a altura do container de navegação entre as rotas
   */
  getContainerHeight(): number {
    return window.innerHeight;
  },
  getContainerWidth(): number {
    return window.innerWidth;
  },
  /**
   * Retorna a altura do container de table
   */
  getHeightTable(): any {
    return this.getContainerHeight() - this.getContainerHeight() * 0.25;
  },
};

export default LayoutService;
