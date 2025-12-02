module argParser

type Args =
    { projectRoot: string
      dllName: string
      nameSpace: string }

let parse (args: string[]) =
    let projectRoot = args[0]
    let dllName = args[1]
    let nameSpace = args[2]

    { projectRoot = projectRoot
      dllName = dllName
      nameSpace = nameSpace }
