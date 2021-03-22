var camera;
var renderer;
var controls;
var viewModel;
var itemMaterial;

var width = 0;
var height = 0;

async function PackContainers(request) {
    return $.ajax({
        url: '/api/containerpacking',
        type: 'POST',
        data: request,
        contentType: 'application/json; charset=utf-8'
    });
};
function InitializeDrawing(wid, hei) {
    var container = $('#drawing-container');

    scene = new THREE.Scene();
    camera = new THREE.PerspectiveCamera(50, window.innerWidth  / window.innerHeight, 0.1, 1000);
    camera.lookAt(scene.position);

    //var axisHelper = new THREE.AxisHelper( 5 );
    //scene.add( axisHelper );

    // LIGHT
    var light = new THREE.PointLight(0xffffff);
    light.position.set(0, 150, 100);
    scene.add(light);

    // Get the item stuff ready.
    //itemMaterial = new THREE.MeshNormalMaterial({  color: 'red' ,transparent: false, opacity: 0.6});
    itemMaterial = new THREE.MeshLambertMaterial({ color: 'blue', transparent: true, opacity: 0.6 });

    renderer = new THREE.WebGLRenderer({ antialias: true }); // WebGLRenderer CanvasRenderer
    renderer.setClearColor(0xf0f0f0);
    renderer.setPixelRatio(window.devicePixelRatio);
    if (wid > 0 && hei > 0) {
        width = wid;
        height = hei;
    }
    else {
        width = window.innerWidth / 2;
        height = window.innerHeight / 2;
    }
    renderer.setSize(width, height);


    container.append(renderer.domElement);

    controls = new THREE.OrbitControls(camera, renderer.domElement);
    window.addEventListener('resize', onWindowResize, false);

    animate();
};
function onWindowResize() {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(width, height);
}
function animate() {
    requestAnimationFrame(animate);
    controls.update();
    render();
}
function render() {
    renderer.render(scene, camera);
}
function Show3d(Carga) {

    Carregando.abrir('Processando....');
    $.ajax({
        url: "/PlugAndPlay/APS/setAvaliarCargaTotal?strIds=" + Carga,
        type: "GET",
        //data: { strIds: Carga },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            Carregando.fechar();

            if (r.st == "PK") {
                $('#carIdOnIndex').val(Carga);
                $('#tipoOnIndex').val(r.idT);
                $('#traOnIndex').val(r.TRA);
                $('#docaOnIndex').val(r.DOCA);
                $('#placaOnIndex').val(r.PLACA);
                $('#statusOnIndex').val(r.STATUS);
                neglobal = r.NaoEmbarcadas;
                $('#d_PCVP').text(r.PCVP);
                $('#d_TTINE').text(r.TTINE);
                $('#d_TOTItens').text(r.TOTItens);
                var container = r.ct;
                var selectedObject = scene.getObjectByName('container');
                scene.remove(selectedObject);

                for (var i = 0; i < 1000; i++) {
                    var selectedObject = scene.getObjectByName('cube' + i);
                    scene.remove(selectedObject);
                }

                //camera.position.set(container.Length(), container.Length(), container.Length());
                camera.position.set(3, 2, 2);

                viewModel.ItemsToRender = r.itensToPack;
                //self.ItemsToRender(r.itensToPack);
                viewModel.LastItemRenderedIndex = -1;


                viewModel.ContainerOriginOffset.x = -1 * container.Length / 2;
                viewModel.ContainerOriginOffset.y = -1 * container.Height / 2;
                viewModel.ContainerOriginOffset.z = -1 * container.Width / 2;

                var geometry = new THREE.BoxGeometry(container.Length, container.Height, container.Width);
                var geo = new THREE.EdgesGeometry(geometry); // or WireframeGeometry( geometry )
                var mat = new THREE.LineBasicMaterial({ color: 0x000000, linewidth: 2 });
                var wireframe = new THREE.LineSegments(geo, mat);
                wireframe.position.set(0, 0, 0);
                wireframe.name = 'container';
                scene.add(wireframe);
                $('#modalCubagem').modal('show');
            }
            else {
                return r.st;
            }
        },
        error: function () {
            return "Erro nao tratado";
        },
        complete: function (r) {
            Carregando.fechar();
        }
    });
}
function Show3dNovaCarga(Carga) {

    if (Carga.st == "PK") {
        var car = Carga.strIds;
        if (!car.includes(";")) {
            car = car.replace("','", ";");
            car = car.substring(0, car.length - 1);
        }
        $('#carIdOnIndex').val(car);
        neglobal = Carga.NaoEmbarcadas;//nao embarcadas
        neTotItens = Carga.Qtd_Itens_NE;
        $('#d_PCVP').text(Carga.PCVP);
        $('#d_TTINE').text(Carga.TTINE);
        $('#d_TOTItens').text(Carga.TOTItens);

        var container = Carga.ct;
        var selectedObject = scene.getObjectByName('container');
        scene.remove(selectedObject);

        for (var i = 0; i < 1000; i++) {
            var selectedObject = scene.getObjectByName('cube' + i);
            scene.remove(selectedObject);
        }
        camera.position.set(3, 2, 2);

        viewModel.ItemsToRender = Carga.itensToPack;

        viewModel.LastItemRenderedIndex = -1;

        viewModel.ContainerOriginOffset.x = -1 * container.Length / 2;
        viewModel.ContainerOriginOffset.y = -1 * container.Height / 2;
        viewModel.ContainerOriginOffset.z = -1 * container.Width / 2;

        var geometry = new THREE.BoxGeometry(container.Length, container.Height, container.Width);
        var geo = new THREE.EdgesGeometry(geometry); // or WireframeGeometry( geometry )
        var mat = new THREE.LineBasicMaterial({ color: 0x000000, linewidth: 2 });
        var wireframe = new THREE.LineSegments(geo, mat);
        wireframe.position.set(0, 0, 0);
        wireframe.name = 'container';
        scene.add(wireframe);
        $('#modalgetAddAltCarga').modal('hide');
        $('#modalCubagem').modal('show');
    } else if (Carga.st == "AV") {
        alert("Vol. Container Ocupado [" + Carga.PCVP + "%] \n Vol. Itens Embarcados [" + Carga.PIVP + "%] \n Total de Itens[" + Carga.TOTItens + "]");
    }
    else {
        return Carga.st;
    }
}
var ViewModel = function () {
    var self = this;
    self.ItemCounter = 0;
    self.ContainerCounter = 0;
    self.ItemsToRender = {};//ko.observableArray([]);
    self.LastItemRenderedIndex = -1;// ko.observable(-1);
    self.ContainerOriginOffset = { x: 0, y: 0, z: 0 };
    self.AlgorithmsToUse = {};//ko.observableArray([]);
    self.ItemsToPack = {};//ko.observableArray([]);
    self.Containers = {};//ko.observableArray([]);
    self.NewItemToPack = {};//ko.mapping.fromJS(new ItemToPack());
    self.NewContainer = {};//ko.mapping.fromJS(new Container());
    self.GenerateItemsToPack = function () {
        self.ItemsToPack([]);
        self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1000, Name: 'Item0', Length: 1, Width: 1.2, Height: 1.4, Quantity: 14, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
        self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1000, Name: 'Item1', Length: 1, Width: 1.2, Height: 1.34, Quantity: 15, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
		/*self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1001, Name: 'Item2', Length: 1.2, Width: 1.54, Height: 1.26, Quantity: 7, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
        self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1002, Name: 'Item3', Length: 1, Width: 1.27, Height: 1.4, Quantity: 7, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
		self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1003, Name: 'Item4', Length: 0.8, Width: 1.2, Height: 1.34, Quantity: 9, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
		self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1004, Name: 'Item5', Length: 1, Width: 1.34, Height: 1.4, Quantity: 8, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
        self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1005, Name: 'Item6', Length: 0.9, Width: 2.11, Height: 1.14, Quantity: 7, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
        self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1005, Name: 'Item6', Length: 1.02, Width: 1.3, Height: 1.4, Quantity: 7, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
        self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1005, Name: 'Item6', Length: 1.07, Width: 1.2, Height: 1.4, Quantity: 7, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
        self.ItemsToPack.push(ko.mapping.fromJS({ ID: 1005, Name: 'Item6', Length: 1.2, Width: 1.51, Height: 1.84, Quantity: 7, PRO_ROTACIONA_COMPRIMENTO: 'N', PRO_ROTACIONA_LARGURA: 'N', PRO_ROTACIONA_ALTURA: 'A' }));
        */
    };
    self.GenerateContainers = function () {
        self.Containers([]);
        self.Containers.push(ko.mapping.fromJS({ ID: 1000, Name: 'Box1', Length: 8, Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
        /* self.Containers.push(ko.mapping.fromJS({ ID: 1001, Name: 'Box2', Length: 7, Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
         self.Containers.push(ko.mapping.fromJS({ ID: 1002, Name: 'Box3', Length: 8.5, Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
         self.Containers.push(ko.mapping.fromJS({ ID: 1003, Name: 'Box4', Length: 8.6, Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
         self.Containers.push(ko.mapping.fromJS({ ID: 1004, Name: 'Box5', Length: 9, Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
         self.Containers.push(ko.mapping.fromJS({ ID: 1005, Name: 'Box6',  Length: 10, Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
         self.Containers.push(ko.mapping.fromJS({ ID: 1006, Name: 'Box7',  Length: 11, Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
         self.Containers.push(ko.mapping.fromJS({ ID: 1007, Name: 'Box8',  Length: 12,  Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
         self.Containers.push(ko.mapping.fromJS({ ID: 1008, Name: 'Box9',  Length: 14,  Width: 2.4, Height: 2.8, AlgorithmPackingResults: [], AlgorithmPackingResultsI: [] }));
         */
    };
    self.AddAlgorithmToUse = function () {
        var algorithmID = $('#algorithm-select option:selected').val();
        var algorithmName = $('#algorithm-select option:selected').text();
        self.AlgorithmsToUse.push({ AlgorithmID: algorithmID, AlgorithmName: algorithmName });
    };
    self.RemoveAlgorithmToUse = function (item) {
        self.AlgorithmsToUse.remove(item);
    };
    self.AddNewItemToPack = function () {
        self.NewItemToPack.ID(self.ItemCounter++);
        self.ItemsToPack.push(ko.mapping.fromJS(ko.mapping.toJS(self.NewItemToPack)));
        self.NewItemToPack.Name('');
        self.NewItemToPack.Length('');
        self.NewItemToPack.Width('');
        self.NewItemToPack.Height('');
        self.NewItemToPack.Quantity('');
        self.NewItemToPack.PRO_ROTACIONA_COMPRIMENTO('');
        self.NewItemToPack.PRO_ROTACIONA_LARGURA('');
        self.NewItemToPack.PRO_ROTACIONA_ALTURA('');

    };
    self.RemoveItemToPack = function (item) {
        self.ItemsToPack.remove(item);
    };
    self.AddNewContainer = function () {
        self.NewContainer.ID(self.ContainerCounter++);
        self.Containers.push(ko.mapping.fromJS(ko.mapping.toJS(self.NewContainer)));
        self.NewContainer.Name('');
        self.NewContainer.Length('');
        self.NewContainer.Width('');
        self.NewContainer.Height('');



    };
    self.RemoveContainer = function (item) {
        self.Containers.remove(item);
    };
    self.PackContainers = function () {
        var algorithmsToUse = [];

        self.AlgorithmsToUse().forEach(algorithm => {
            algorithmsToUse.push(algorithm.AlgorithmID);
        });

        var itemsToPack = [];

        self.ItemsToPack().forEach(item => {
            var itemToPack = {
                ID: item.ID(),
                Dim1: item.Length(),
                Dim2: item.Width(),
                Dim3: item.Height(),
                Quantity: item.Quantity(),
                PRO_ROTACIONA_COMPRIMENTO: item.PRO_ROTACIONA_COMPRIMENTO(),
                PRO_ROTACIONA_LARGURA: item.PRO_ROTACIONA_LARGURA(),
                PRO_ROTACIONA_ALTURA: item.PRO_ROTACIONA_ALTURA()
            };

            itemsToPack.push(itemToPack);
        });

        var containers = [];

        // Send a packing request for each container in the list.
        self.Containers().forEach(container => {
            var containerToUse = {
                ID: container.ID(),
                Length: container.Length(),
                Width: container.Width(),
                Height: container.Height()
            };

            containers.push(containerToUse);
        });

        // Build container packing request.
        var request = {
            Containers: containers,
            ItemsToPack: itemsToPack,
            AlgorithmTypeIDs: algorithmsToUse
        };

        PackContainers(JSON.stringify(request))
            .then(response => {
                // Tie this response back to the correct containers.
                response.forEach(containerPackingResult => {
                    self.Containers().forEach(container => {
                        if (container.ID() == containerPackingResult.ContainerID) {
                            if (containerPackingResult.ContainerOrientation == "O") {
                                container.AlgorithmPackingResults(containerPackingResult.AlgorithmPackingResults);
                            } else {
                                container.AlgorithmPackingResultsI(containerPackingResult.AlgorithmPackingResults);
                            }
                        }
                    });
                });
            });
    };
    self.ShowPackingView = function (algorithmPackingResult) {
        var container = this;
        var selectedObject = scene.getObjectByName('container');
        scene.remove(selectedObject);

        for (var i = 0; i < 1000; i++) {
            var selectedObject = scene.getObjectByName('cube' + i);
            scene.remove(selectedObject);
        }

        camera.position.set(container.Length(), container.Length(), container.Length());

        self.ItemsToRender(algorithmPackingResult.PackedItems);
        self.LastItemRenderedIndex(-1);

        self.ContainerOriginOffset.x = -1 * container.Length() / 2;
        self.ContainerOriginOffset.y = -1 * container.Height() / 2;
        self.ContainerOriginOffset.z = -1 * container.Width() / 2;

        var geometry = new THREE.BoxGeometry(container.Length(), container.Height(), container.Width());
        var geo = new THREE.EdgesGeometry(geometry); // or WireframeGeometry( geometry )
        var mat = new THREE.LineBasicMaterial({ color: 0x000000, linewidth: 2 });
        var wireframe = new THREE.LineSegments(geo, mat);
        wireframe.position.set(0, 0, 0);
        wireframe.name = 'container';
        scene.add(wireframe);
    };
    self.AreItemsPacked = function () {
        if (self.LastItemRenderedIndex() > -1) {
            return true;
        }

        return false;
    };

    self.AreAllItemsPacked = function () {
        if (self.ItemsToRender().length === self.LastItemRenderedIndex() + 1) {
            return true;
        }

        return false;
    };

    self.PackItemInRender = function () {
        var itemIndex = self.LastItemRenderedIndex + 1;

        var itemOriginOffset = {
            x: self.ItemsToRender[itemIndex].PackDimX / 2,
            y: self.ItemsToRender[itemIndex].PackDimY / 2,
            z: self.ItemsToRender[itemIndex].PackDimZ / 2
        };

        var itemGeometry = new THREE.BoxGeometry(self.ItemsToRender[itemIndex].PackDimX, self.ItemsToRender[itemIndex].PackDimY, self.ItemsToRender[itemIndex].PackDimZ);

        itemMaterial.color.setHSL(200, 100, itemIndex);  // red
        itemMaterial.flatShading = true;


        var cube = new THREE.Mesh(itemGeometry, itemMaterial);
        cube.position.set(self.ContainerOriginOffset.x + itemOriginOffset.x + self.ItemsToRender[itemIndex].CoordX, self.ContainerOriginOffset.y + itemOriginOffset.y + self.ItemsToRender[itemIndex].CoordY, self.ContainerOriginOffset.z + itemOriginOffset.z + self.ItemsToRender[itemIndex].CoordZ);
        cube.name = 'cube' + itemIndex;
        scene.add(cube);

        self.LastItemRenderedIndex = itemIndex;
    };

    self.UnpackItemInRender = function () {
        var selectedObject = scene.getObjectByName('cube' + self.LastItemRenderedIndex);
        scene.remove(selectedObject);
        self.LastItemRenderedIndex = self.LastItemRenderedIndex - 1;
        if (self.LastItemRenderedIndex < -1) {
            self.LastItemRenderedIndex = -1;
        }

    };
};
var ItemToPack = function () {
    this.ID = '';
    this.Name = '';
    this.Length = '';
    this.Width = '';
    this.Height = '',
        this.Quantity = '';
    this.PRO_ROTACIONA_COMPRIMENTO = '';
    this.PRO_ROTACIONA_LARGURA = '';
    this.PRO_ROTACIONA_ALTURA = '';

}
var Container = function () {
    this.ID = '';
    this.Name = '';
    this.Length = '';
    this.Width = '';
    this.Height = '';
    this.AlgorithmPackingResults = [];
}
$(document).ready(() => {
    $('[data-toggle="tooltip"]').tooltip();
    var tamanho = $('#drawing-container').attr('class'); //Lá onde você declarou #drawing-container se você declarar a classe tamanho_x_y o X e Y serão o tamanho do retângulo que será desenhado.
    var width = 0;
    var height = 0;

    if (tamanho != undefined) {
        width = tamanho.split('_')[1];
        height = tamanho.split('_')[2];
    }
    InitializeDrawing(width, height);

    viewModel = new ViewModel();
    // ko.applyBindings(viewModel);
});