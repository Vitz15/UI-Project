-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 25, 2024 at 09:59 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_apotek`
--

-- --------------------------------------------------------

--
-- Table structure for table `admin`
--

CREATE TABLE `admin` (
  `id_admin` int(11) NOT NULL,
  `nama_lengkap` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `username` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL,
  `no_telp` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `admin`
--

INSERT INTO `admin` (`id_admin`, `nama_lengkap`, `password`, `username`, `email`, `no_telp`) VALUES
(1, 'Administrator', 'admin', 'admin', 'admin@gmail.com', '084758374574'),
(2, 'Matthew Mokolensang', 'MatthewGanteng', 'Matthew', 'MatthewGanteng@gmail.com', '087534546534');

-- --------------------------------------------------------

--
-- Table structure for table `laporan_keuangan`
--

CREATE TABLE `laporan_keuangan` (
  `tgl_laporan` date NOT NULL,
  `total_pendapatan` decimal(15,2) NOT NULL,
  `total_pengeluaran` decimal(15,2) NOT NULL,
  `laba_bersih` decimal(15,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `laporan_keuangan`
--

INSERT INTO `laporan_keuangan` (`tgl_laporan`, `total_pendapatan`, `total_pengeluaran`, `laba_bersih`) VALUES
('2024-11-30', 1200000.00, 200000.00, 1000000.00),
('2024-11-30', 1200000.00, 200000.00, 1000000.00),
('2024-11-30', 1200000.00, 200000.00, 1000000.00),
('2024-11-30', 1000000.00, 200000.00, 800000.00),
('2024-11-30', 0.00, 0.00, 0.00),
('2024-11-30', 12000.00, 200000.00, -188000.00),
('2024-10-31', 0.00, 0.00, 0.00),
('2023-10-31', 0.00, 0.00, 0.00),
('2024-11-30', 92000.00, 300000.00, -208000.00);

-- --------------------------------------------------------

--
-- Table structure for table `obat`
--

CREATE TABLE `obat` (
  `id_obat` int(11) NOT NULL,
  `tgl_masuk` date NOT NULL,
  `nama_obat` varchar(50) NOT NULL,
  `kategori` varchar(50) NOT NULL,
  `harga_jual` int(50) NOT NULL,
  `stok` int(50) NOT NULL,
  `satuan` varchar(50) NOT NULL,
  `tanggal_kadaluarsa` date NOT NULL,
  `deskripsi` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `obat`
--

INSERT INTO `obat` (`id_obat`, `tgl_masuk`, `nama_obat`, `kategori`, `harga_jual`, `stok`, `satuan`, `tanggal_kadaluarsa`, `deskripsi`) VALUES
(154, '2024-11-15', 'paracetamol', 'keras', 8000, 10, 'ml', '2024-11-29', 'mantap');

-- --------------------------------------------------------

--
-- Table structure for table `pelanggan`
--

CREATE TABLE `pelanggan` (
  `id_pelanggan` varchar(50) NOT NULL,
  `nama_pelanggan` varchar(50) NOT NULL,
  `alamat` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL,
  `no_telp` varchar(50) NOT NULL,
  `tgl_registrasi` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pelanggan`
--

INSERT INTO `pelanggan` (`id_pelanggan`, `nama_pelanggan`, `alamat`, `email`, `no_telp`, `tgl_registrasi`) VALUES
('SUB001', 'Deeva', 'tumaluntung', 'deeva@gmail.com', '08134438867', '2024-11-11'),
('SUB002', 'brandon', 'airmadidi', 'brandon@gmail.com', '08123456678', '2024-11-13');

-- --------------------------------------------------------

--
-- Table structure for table `pelanggan_umum`
--

CREATE TABLE `pelanggan_umum` (
  `Id` int(50) NOT NULL,
  `nama_pelanggan` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pelanggan_umum`
--

INSERT INTO `pelanggan_umum` (`Id`, `nama_pelanggan`) VALUES
(1, 'Pelanggan Umum');

-- --------------------------------------------------------

--
-- Table structure for table `pemusnahan`
--

CREATE TABLE `pemusnahan` (
  `nama_obat` varchar(50) NOT NULL,
  `tgl_kadaluarsa` date NOT NULL,
  `jumlah` int(50) NOT NULL,
  `deskripsi` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pemusnahan`
--

INSERT INTO `pemusnahan` (`nama_obat`, `tgl_kadaluarsa`, `jumlah`, `deskripsi`) VALUES
('Paracetamol', '2024-11-13', 50, 'Expired'),
('Paracetamol', '2024-11-30', 17, 'dihapus dari kemenkes'),
('paracetamol', '2024-11-23', 10, 'Expired');

-- --------------------------------------------------------

--
-- Table structure for table `stok_masuk`
--

CREATE TABLE `stok_masuk` (
  `id_stok_masuk` int(11) NOT NULL,
  `tgl_masuk` date NOT NULL,
  `nama_obat` varchar(50) NOT NULL,
  `jumlah` int(50) NOT NULL,
  `harga_beli_satuan` int(50) NOT NULL,
  `total_harga_beli` int(50) NOT NULL,
  `tgl_kadaluarsa` date NOT NULL,
  `supplier` varchar(50) NOT NULL,
  `harga_jual` int(50) NOT NULL,
  `nomor_bench` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `stok_masuk`
--

INSERT INTO `stok_masuk` (`id_stok_masuk`, `tgl_masuk`, `nama_obat`, `jumlah`, `harga_beli_satuan`, `total_harga_beli`, `tgl_kadaluarsa`, `supplier`, `harga_jual`, `nomor_bench`) VALUES
(70, '2024-11-15', 'paracetamol', 20, 6000, 300000, '2024-11-30', 'Bran', 8000, '1');

-- --------------------------------------------------------

--
-- Table structure for table `transaksi`
--

CREATE TABLE `transaksi` (
  `id_transaksi` int(11) NOT NULL,
  `nama_pelanggan` varchar(50) NOT NULL,
  `nama_obat` varchar(50) NOT NULL,
  `tgl_transaksi` date NOT NULL,
  `jumlah` int(50) NOT NULL,
  `harga_satuan` int(50) NOT NULL,
  `total_harga` int(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `transaksi`
--

INSERT INTO `transaksi` (`id_transaksi`, `nama_pelanggan`, `nama_obat`, `tgl_transaksi`, `jumlah`, `harga_satuan`, `total_harga`) VALUES
(53, 'brandon', 'Paracetamol', '2024-11-13', 3, 4000, 12000),
(54, 'Pelanggan Umum', 'paracetamol', '2024-11-23', 10, 8000, 80000);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `admin`
--
ALTER TABLE `admin`
  ADD PRIMARY KEY (`id_admin`);

--
-- Indexes for table `obat`
--
ALTER TABLE `obat`
  ADD PRIMARY KEY (`id_obat`);

--
-- Indexes for table `pelanggan`
--
ALTER TABLE `pelanggan`
  ADD PRIMARY KEY (`id_pelanggan`);

--
-- Indexes for table `pelanggan_umum`
--
ALTER TABLE `pelanggan_umum`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `stok_masuk`
--
ALTER TABLE `stok_masuk`
  ADD PRIMARY KEY (`id_stok_masuk`);

--
-- Indexes for table `transaksi`
--
ALTER TABLE `transaksi`
  ADD PRIMARY KEY (`id_transaksi`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `admin`
--
ALTER TABLE `admin`
  MODIFY `id_admin` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `obat`
--
ALTER TABLE `obat`
  MODIFY `id_obat` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=156;

--
-- AUTO_INCREMENT for table `pelanggan_umum`
--
ALTER TABLE `pelanggan_umum`
  MODIFY `Id` int(50) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `stok_masuk`
--
ALTER TABLE `stok_masuk`
  MODIFY `id_stok_masuk` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=71;

--
-- AUTO_INCREMENT for table `transaksi`
--
ALTER TABLE `transaksi`
  MODIFY `id_transaksi` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
