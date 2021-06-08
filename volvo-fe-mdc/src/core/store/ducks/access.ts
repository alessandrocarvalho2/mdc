const INITIAL_STATE = {
  data: []
}

const access = (state = INITIAL_STATE, action: any) => {
  if (action.type === 'ADD_ACCESS') {
    return  { ...state, data: [ ...state.data, action.access ] } 
  }
  return state;
} 

export default access;