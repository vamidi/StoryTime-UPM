using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("com.vamidicreations.storytime.runtime")]
[assembly: InternalsVisibleTo("com.vamidicreations.storytime.editor")]

[assembly: InternalsVisibleTo("com.vamidicreations.storytime.runtime.visualscripting")]
[assembly: InternalsVisibleTo("com.vamidicreations.storytime.editor.visualscripting")]

[assembly: InternalsVisibleTo("com.vamidicreations.storytime.firebase.database")]
[assembly: InternalsVisibleTo("com.vamidicreations.storytime.firebase.database.editor")]

[assembly: InternalsVisibleTo("com.vamidicreations.storytime.editor.tests.project")]
[assembly: InternalsVisibleTo("com.vamidicreations.storytime.editor.tests.performance")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] // Required for faking internal classes
