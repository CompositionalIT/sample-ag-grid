module Client

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json
open Fable.FontAwesome

open Shared

module AgGrid =
    open Fable.Core
    open Fable.Core.JsInterop

    type ColumnDef =
        { headerName : string
          field : string
          sortable : bool
          filter : bool;
          checkboxSelection : bool }
        static member Create headerName field = { headerName = headerName; field = field; sortable = false; filter = false; checkboxSelection = false }

    type Props =
        | ColumnDefs of ColumnDef array
        | RowData of obj array

    let inline grid (props : Props list) (elems : ReactElement list) : ReactElement =
        ofImport "AgGridReact" "ag-grid-react" (keyValueList CaseRules.LowerFirst props) elems

type Model = { Counter: Counter option }
type Data = { name: string; uv: int; pv: int; amt: int }
open AgGrid

let init() = (), Cmd.none
let update _ _ = (), Cmd.none

let data =
    [| { name = "Page A"; uv = 4000; pv = 2400; amt = 2400 }
       { name = "Page B"; uv = 3000; pv = 1398; amt = 2210 }
       { name = "Page C"; uv = 2000; pv = 9800; amt = 2290 }
       { name = "Page D"; uv = 2780; pv = 3908; amt = 2000 }
       { name = "Page E"; uv = 1890; pv = 4800; amt = 2181 }
       { name = "Page F"; uv = 2390; pv = 3800; amt = 2500 }
       { name = "Page G"; uv = 3490; pv = 4300; amt = 2100 } |]
let view (model : _) (dispatch : _ -> unit) =
    div [] [
        Navbar.navbar [
            Navbar.Color IsPrimary
        ] [
            Navbar.Item.div [ ] [
                Heading.h2 [ ] [
                    str "SAFE Template"
                ]
            ]
        ]

        Container.container [] [
            Columns.columns [ ] [
                Column.column [] [
                    div [ Id "myGrid"; Class "ag-theme-balham"; Style [ Height "200px"; Width "500px"  ] ] [
                        grid [
                            Props.ColumnDefs [|
                                { ColumnDef.Create "Name" "name" with filter = true; checkboxSelection = true }
                                { ColumnDef.Create "UV" "uv" with sortable = true }
                            |]
                            Props.RowData (data |> Array.map box)
                        ] [

                        ]
                    ]
                ]
            ]
        ]

        Footer.footer [ ] [
            Content.content [
                Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ]
            ] [

            ]
        ]
    ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
