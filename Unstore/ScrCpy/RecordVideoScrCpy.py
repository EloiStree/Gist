import subprocess
import time

p = subprocess.Popen(['scrcpy', '--record', 'haha.mkv'])
time.sleep(600)
p.kill()
