const validation: any = {
  /**
   * Checks if number
   */
  isNumber(n: any): boolean {
    return /^-?[\d.]+(?:e-?\d+)?$/.test(n);
  },
  isOnlyNumberHyphensSemicolon(n: any): boolean {
    return /^[0-9-;]+$/.test(n);
  },
  isDate(n: any): boolean {
    return /^(\d{2})\/(\d{2})\/(\d{4})$/.test(n);
  },
};

export default validation;
