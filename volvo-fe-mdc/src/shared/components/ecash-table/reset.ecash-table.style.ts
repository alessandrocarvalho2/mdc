import { createMuiTheme } from "@material-ui/core/styles";

//
const resetMaterialTable = createMuiTheme({
  palette: {
    primary: {
      main: '#003A7B',
      light: '#5C83AF',
      contrastText: '#fff',
    },
    secondary: {
      main: '#D2040A',
      light: '#FF5E63',
      contrastText: '#fff',
    },
    text: {
      primary: 'rgba(0, 0, 0, 0.87)',
      secondary: 'rgba(0, 0, 0, 0.54)',
      disabled: 'rgba(0, 0, 0, 0.38)',
      hint: 'rgba(0, 0, 0, 0.38)',
    },
  },
  overrides: {
    MuiPaper: {
      root: {
        boxShadow: 'none !important'
      }
    }
  }
});
//

export default resetMaterialTable;
