using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using TostaoApp.Actividades;
using TostaoApp.DataHelper;
using TostaoBeta1;

namespace TostaoApp.Clases
{
    class RecyclerViewHolder : RecyclerView.ViewHolder
    {
        public ImageView imageView { get; set; }
        public TextView txtDescription { get; set; }
        public TextView txtPrecio { get; set; }
        public Button botonMas { get; set; }
        public Button botonMenos { get; set; }
        public TextView cantidadProducto { get; set; }
        public LinearLayout item { get; set; }

        public RecyclerViewHolder(View itemView): base(itemView) {
            imageView = itemView.FindViewById<ImageView>(Resource.Id.imageView);
            txtDescription = itemView.FindViewById<TextView>(Resource.Id.txtDescription);
            txtPrecio = itemView.FindViewById<TextView>(Resource.Id.txtPrice);
            botonMas = itemView.FindViewById<Button>(Resource.Id.botonMas);
            botonMenos = itemView.FindViewById<Button>(Resource.Id.botonMenos);
            cantidadProducto = itemView.FindViewById<TextView>(Resource.Id.cantidadProducto);
            item = itemView.FindViewById<LinearLayout>(Resource.Id.item);
        }
    }

    class RecyclerViewAdapter : RecyclerView.Adapter
    {
        private List<Data> lstData = new List<Data>();
        public int positionScroll = 0;
        Activity _context;
        DataBase db;

        public RecyclerViewAdapter(List<Data> lstData, Activity context)
        {
            this.lstData = lstData;
            _context = context;
            db = new DataBase();
        }

        public override int ItemCount
        {
            get {
                return lstData.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewHolder viewHolder = holder as RecyclerViewHolder;
            positionScroll = viewHolder.AdapterPosition;

            // ** IMPORTANTE **
            // La variable position posee los elementos que se van agregando en la posición actual del recyclerView
            // y es la posición del reciclaje de la vista en esa fila.
            // La variable viewHolder.position es la variable de la posicion absoluta de una celda respecto al recyclerView
            // por lo que si se quieren hacer cambios a una celda, se debe usar esta posición, y si se quiere agregar información
            // por primera vez, se debe usar solo position
            // El intentar usar viewHolder.position dentro de otra variable o mandandose como parámetro tiene resultados inesperados
            // por lo que hay bastante repetición de código en esta función

            viewHolder.imageView.SetImageResource(lstData[position].imageId);
            viewHolder.txtDescription.Text = lstData[position].description;
            viewHolder.cantidadProducto.Text = lstData[viewHolder.AdapterPosition].cantidadProducto + "";


            TextView cantidadProductos, cantidadCompra;
            cantidadProductos = _context.FindViewById<TextView>(Resource.Id.cantidadProductos);
            cantidadCompra = _context.FindViewById<TextView>(Resource.Id.totalCompra);

            // Si la clase es carro de compra, se agregan productos
            if (_context.Class.SimpleName.Equals(typeof(CarroDeCompra).Name))
            {
                viewHolder.txtPrecio.Text = "$" + (lstData[viewHolder.AdapterPosition].precioProducto * lstData[viewHolder.AdapterPosition].cantidadProducto);
            }
            else
            {
                viewHolder.txtPrecio.Text = "$" + (lstData[viewHolder.AdapterPosition].precioProducto);
            }

            //  Click sobre el Producto, abre la actividad del Producto
            
            if (!viewHolder.item.HasOnClickListeners)
            {
                viewHolder.item.Click += delegate
                {
                    var intent = new Intent(_context, typeof(Actividades.ProductoActividad));
                    intent.PutExtra("Producto", lstData[viewHolder.AdapterPosition].productId);
                    _context.StartActivity(intent);
                };
            }

            //  Click sobre los botones ( + , -) de los productos
            if (!viewHolder.botonMenos.HasOnClickListeners && !viewHolder.botonMas.HasOnClickListeners && viewHolder.AdapterPosition >= 0)
            {
                viewHolder.botonMenos.Click += delegate
                {
                    //Solo se eliminan productos si la cantidad es mayor a 0, esto evita valores negativos
                    if (lstData[viewHolder.AdapterPosition].cantidadProducto > 0)
                    {

                        lstData[viewHolder.AdapterPosition].cantidadProducto--;
                        viewHolder.cantidadProducto.Text = lstData[viewHolder.AdapterPosition].cantidadProducto + "";
                        db.updateTableProducto(lstData[viewHolder.AdapterPosition].cantidadProducto, lstData[viewHolder.AdapterPosition].productId);

                        // Se cambia el precio mostrado al disminuir la cantidad solo si es CarroDeCompra
                        if (_context.Class.SimpleName.Equals(typeof(CarroDeCompra).Name))
                        {
                            viewHolder.txtPrecio.Text = "$" + (lstData[viewHolder.AdapterPosition].precioProducto * lstData[viewHolder.AdapterPosition].cantidadProducto);
                        }

                        // Se elimina el producto solo si la cantidad es 0 y solo si es CarroDeCompra
                        if (_context.Class.SimpleName.Equals(typeof(CarroDeCompra).Name) && lstData[viewHolder.AdapterPosition].cantidadProducto == 0)
                        {
                            lstData.Remove(lstData[viewHolder.AdapterPosition]);
                            removeAt(viewHolder, viewHolder.AdapterPosition);
                            NotifyItemRemoved(viewHolder.AdapterPosition);
                        }

                        List<Producto> CarroDeCompra = db.selectQueryTableProductoParaCarro();
                        cantidadProductos.Text = TotalProductos(CarroDeCompra)[0] + "";
                        cantidadCompra.Text = "$" + TotalProductos(CarroDeCompra)[1];
                    }
                };

                viewHolder.botonMas.Click += delegate
                {

                    lstData[viewHolder.AdapterPosition].cantidadProducto++;
                    viewHolder.cantidadProducto.Text = lstData[viewHolder.AdapterPosition].cantidadProducto + "";
                    db.updateTableProducto(lstData[viewHolder.AdapterPosition].cantidadProducto, lstData[viewHolder.AdapterPosition].productId);
                    
                    // Se cambia el precio mostrado al aumentar la cantidad solo si es CarroDeCompra
                    if (_context.Class.SimpleName.Equals(typeof(CarroDeCompra).Name))
                    {
                        viewHolder.txtPrecio.Text = "$" + (lstData[viewHolder.AdapterPosition].precioProducto * lstData[viewHolder.AdapterPosition].cantidadProducto);
                    }
                    
                    List<Producto> CarroDeCompra = db.selectQueryTableProductoParaCarro();
                    cantidadProductos.Text = TotalProductos(CarroDeCompra)[0] + "";
                    cantidadCompra.Text = "$" + TotalProductos(CarroDeCompra)[1];
                };
            }
        }

        public void ProductAddClick(Data data, int add, RecyclerViewHolder viewHolder)
        {
            data.cantidadProducto += add;
            viewHolder.cantidadProducto.Text = data.cantidadProducto + "";
            db.updateTableProducto(data.cantidadProducto, data.productId);
        }

        private int[] TotalProductos(List<Producto> CarroDeCompra)
        {
            int cantidadProductos = 0;
            int precioTotal = 0;
            foreach (Producto producto in CarroDeCompra)
            {
                cantidadProductos += producto.Cantidad;
                precioTotal += producto.Cantidad * producto.Precio;
            }
            return new int[] { cantidadProductos, precioTotal };
        }

        public void callit()
        {
            Toast.MakeText(_context, "Si work", ToastLength.Short).Show();
        }
        
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.item, parent, false);
            RecyclerViewHolder vm = new RecyclerViewHolder(itemView);
            return vm;
        }

        public void removeAt(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewHolder viewHolder = holder as RecyclerViewHolder;
        }
    }
}