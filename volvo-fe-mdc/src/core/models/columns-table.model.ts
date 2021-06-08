export interface Columns {
    id: string;
    label: string;
    width?: string;
    orderDefault?: string;
    align?: "right" | "center" | "left";
    disablePadding?: "none" | "default"
    applyColorMonetary?: boolean
    witnCheckedAll?: boolean
    type?: "index" |"string" | "textfield" | "checkbox" | "monetary" | "button";
    icon?: "delete" |"edit";
    format?: (value: any) => string;
    children?: Columns[];
  }
  export default Columns;