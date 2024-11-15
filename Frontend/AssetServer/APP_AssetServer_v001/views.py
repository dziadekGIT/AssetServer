import os
from django.shortcuts import render
import requests

def send_post(request):
    response_data = None

    if request.method == 'POST' and request.FILES['imagePath'] and request.FILES['fbxPath']:
        
        name = request.POST.get('name')
        version = request.POST.get('version')
        
 
        image = request.FILES['imagePath']
        fbx = request.FILES['fbxPath']
        
        print("Dane z formularza:", name, version)

        #localhost network
        #url = 'http://localhost:5190/api/AssetServer/Create'
        
        #docker network not :) localhost - @TODO docker network
        backend_url = os.environ.get('BACKEND_URL', 'http://localhost:5190')
        url = f'{backend_url}/api/AssetServer/Create'
  
        files = {
            'imagePath': image,
            'fbxPath': fbx
        }

   
        data = {
            'name': name,
            'version': version
        }

        try:
            response = requests.post(url, data=data, files=files, timeout=10)
            response_data = response.json()
            print("Odpowiedź API:", response_data)
        except Exception as e:
            response_data = {'error': str(e)}
            print("Błąd:", e)

    return render(request, 'frontend/form.html', {'response_data': response_data})
