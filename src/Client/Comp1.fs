module Comp1


open Elmish

type Model = { Comp1: string }

type Msg =
    | SomeComp1Msg

let init () =
    { Comp1 = "" }

let update (state:State.IStateManager) (msg:Msg) (model:Model) =
    let shared = state.State.State
    let cmd = Cmd.ofSub(fun dispatch ->
        // Works, but order might change...
        dispatch(SomeComp1Msg)
        state.StartDecrement ()
        dispatch(SomeComp1Msg))
    // Works    
    state.StartIncrement()
    model, cmd


let view (state:State.IStateManager) (model:Model) (dispatch:Dispatch<Msg>) =
    //
    ()