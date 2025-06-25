// src/App.jsx
import { useState } from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { GalleryHorizontal, Upload } from 'lucide-react';

export default function App() {
  const [selectedFile, setSelectedFile] = useState(null);
  const [textureZip, setTextureZip] = useState(null);
  const [previewImage, setPreviewImage] = useState(null);
  const [modelName, setModelName] = useState('');
  const [assets, setAssets] = useState([]);

  const handleUpload = () => {
    const asset = {
      name: modelName || (selectedFile?.name ?? 'Unnamed model'),
      previewUrl: previewImage ? URL.createObjectURL(previewImage) : 'https://via.placeholder.com/150',
      downloadUrl: selectedFile ? URL.createObjectURL(selectedFile) : '#'
    };
    setAssets([...assets, asset]);
    setModelName('');
    setSelectedFile(null);
    setPreviewImage(null);
    setTextureZip(null);
  };

  return (
    <div className="flex h-screen bg-gradient-to-tr from-gray-100 to-white">
      {/* Sidebar with Upload Form */}
      <aside className="w-96 bg-gray-900 text-white flex flex-col justify-between p-6 rounded-tr-3xl rounded-br-3xl shadow-lg">
        <div>
          <div className="text-2xl font-bold mb-8">AssetServer</div>

          <div className="space-y-4">
            <div className="flex items-center gap-2 text-blue-400 font-semibold">
              <Upload className="w-5 h-5" />
              <span>Add 3d model</span>
            </div>

            <Card className="bg-gray-800 text-white rounded-xl mt-4 shadow-inner">
              <CardContent className="space-y-4 p-4">
                <div>
                  <Label htmlFor="model">Model (.fbx, .obj, .3ds, .max)</Label>
                  <Input
                    id="model"
                    type="file"
                    accept=".fbx,.obj,.3ds,.max"
                    onChange={(e) => setSelectedFile(e.target.files[0])}
                    className="bg-gray-700 text-white"
                  />
                </div>
                <div>
                  <Label htmlFor="textures">Textures (.zip)</Label>
                  <Input
                    id="textures"
                    type="file"
                    accept=".zip"
                    onChange={(e) => setTextureZip(e.target.files[0])}
                    className="bg-gray-700 text-white"
                  />
                </div>
                <div>
                  <Label htmlFor="preview">Preview (picture)</Label>
                  <Input
                    id="preview"
                    type="file"
                    accept="image/*"
                    onChange={(e) => setPreviewImage(e.target.files[0])}
                    className="bg-gray-700 text-white"
                  />
                </div>
                <div>
                  <Label htmlFor="name">Model name</Label>
                  <Input
                    id="name"
                    type="text"
                    placeholder="np. sci-fi_dron_01"
                    value={modelName}
                    onChange={(e) => setModelName(e.target.value)}
                    className="bg-gray-700 text-white"
                  />
                </div>
                <Button onClick={handleUpload}>Upload</Button>
              </CardContent>
            </Card>
          </div>
        </div>

        <footer className="text-sm text-gray-400 mt-8">
          © 2025 Wojciech Stroz
        </footer>
      </aside>

      {/* Main Gallery */}
      <main className="flex-1 p-8 overflow-y-auto">
        <div className="flex items-center gap-2 mb-4 text-gray-800 font-semibold text-xl">
          <GalleryHorizontal className="w-6 h-6" />
          Asset Gallery
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {assets.map((asset, idx) => (
            <Card key={idx} className="rounded-xl shadow-lg hover:scale-[1.02] transition-transform">
              <CardContent className="p-4">
                <img src={asset.previewUrl} alt={asset.name} className="rounded-md" />
                <div className="mt-2 font-semibold">{asset.name}</div>
                <a
                  href={asset.downloadUrl}
                  className="text-sm text-blue-600 hover:underline"
                  download
                >
                  Download
                </a>
              </CardContent>
            </Card>
          ))}
        </div>
      </main>
    </div>
  );
}
