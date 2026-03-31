import os
import subprocess

subprocess.run(['dotnet', 'test'], shell=True)
print(os.path.exists('Tests/bin/Debug/net10.0/Results.csv'))
