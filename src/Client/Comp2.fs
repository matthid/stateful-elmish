module Comp2

open Elmish

type Model = { Comp2: string }

type Msg =
    | SomeComp2Msg

let init () =
    { Comp2 = "" }

let update (state:State.IStateManager) (msg:Msg) (model:Model) =
    let shared = state.State.State
    let cmd = Cmd.ofSub(fun dispatch ->
        // Works, but order might change...
        dispatch(SomeComp2Msg)
        state.StartDecrement ()
        dispatch(SomeComp2Msg))
    // Works    
    state.StartIncrement()
    model, cmd

let view (state:State.IStateManager) (model:Model) (dispatch:Dispatch<Msg>) =
    //
    ()